using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using RestSharp;
using System.Collections.Generic;

//raprock dependencies
using RaprockPlaylist.Context;
using RaprockPlaylist.Models;
using RaprockPlaylist.Factories;
using RaprockPlaylist.Functions;
using System.Text;

namespace RaprockPlaylist.Controllers
{

    public class FormController : Controller
    {
        private string url = "https://www.google.com/recaptcha/api/siteverify";
        private string secretKey = "6LdB_8gUAAAAAOjR8cm667pv5oJLN_mh7soFxuKH";
        private readonly IHttpContextAccessor _accessor;
        private readonly PlaylistContext _context;
        StringBuilder stringBuilder = new StringBuilder();


        public FormController(IHttpContextAccessor accessor, PlaylistContext context)
        {
            _accessor = accessor;
            _context = context;
        }
        public IActionResult RequestForm(string songRequest, string email, string bandName, string bandLocation)
        {
            Visitor visitor = null;
            try
            {
                //validify all input
                if (HttpContext.Session.GetString("captcha") != "verified" || String.IsNullOrEmpty(HttpContext.Session.GetString("captcha")))
                    return BadRequest("Please verify that you are human");
                if (!Functions.Validify.IsValidEmail(email))
                    return BadRequest("Invalid email");
                if (String.IsNullOrEmpty(songRequest) || String.IsNullOrEmpty(bandName) || String.IsNullOrEmpty(bandLocation))
                    return BadRequest("Please fill up all form fields");

                songRequest = HttpUtility.HtmlEncode(songRequest);
                bandName = HttpUtility.HtmlEncode(bandName);
                _context.Database.EnsureCreated();
                visitor = Functions.Log.InitializeVisitor(_context, _accessor);
                //Log
                Functions.Log.LogActivity(_context, "Index", "Sending form", visitor);

                //Check if band exists if not, create one
                Band band = _context.Band.Where(v => v.BandName == bandName).FirstOrDefault() ?? new Band();
                if (String.IsNullOrEmpty(band.BandName))
                {
                    band = new Band();
                    band.BandName = bandName;
                    band.BandLocation = bandLocation;
                    _context.Band.Add(band);
                }
                
                //Create new song request
                SongRequest newSongRequest = new SongRequest
                {
                    SongRequest1 = songRequest,
                    IdVisitorNavigation = visitor,
                    Email = email
                };

                _context.SongRequest.Add(newSongRequest);

                Functions.Mail.SendMail("New song request", stringBuilder.Append("Band: ").Append(bandName).Append(" - From: ").Append(bandLocation).Append("<br>Message:<br>").Append(songRequest).ToString(), email);
                _context.SaveChanges();
                return Ok("Thank you!");
            }
            catch (Exception e)
            {
                Functions.Log.LogError(_context, "Index-Send Form", e.ToString(), visitor ?? Functions.Log.InitializeVisitor(_context, _accessor));
                return BadRequest();
            }
        }
        public StatusCodeResult ReCaptchaVerify(string captchaResponse)
        {
            Visitor visitor = null;
            try
            {
                IRestResponse reCaptcha;
                _context.Database.EnsureCreated();
                visitor = Functions.Log.InitializeVisitor(_context,_accessor);
                //log
                Functions.Log.LogActivity(_context, "g-recaptchaSend", "Sending captcha for verification", visitor);

                //reCaptcha verify
                RequestFactory RC = new RequestFactory();
                RestRequest restRequest = RC.RequestConstructor(
                    new Dictionary<string, string>()
                        {
                            {"secret",secretKey},
                            {"response", captchaResponse},
                            {"remoteip", visitor.IpAdress}
                        },
                        Method.POST);
                reCaptcha = RC.RequestSender(url, restRequest);

                //log
                Functions.Log.LogActivity(_context, "g-recaptchaSend", "Received captcha for verification", visitor);

                _context.SaveChanges();


                //Check if captcha was successful or not
                //if(reCaptcha.Content.Contains('"'+"success"+'"'+": true"))
                if (reCaptcha.Content.Contains("true"))
                {
                    HttpContext.Session.SetString("captcha", "verified");
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Functions.Log.LogError(_context, "Index-Captcha verify", e.ToString(), visitor ?? Functions.Log.InitializeVisitor(_context, _accessor));
            }
            return BadRequest();
        }
    }
}
