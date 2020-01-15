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
        private Log log;
        StringBuilder stringBuilder = new StringBuilder();
                

        public FormController(IHttpContextAccessor accessor,PlaylistContext context)
        {
            _accessor = accessor;
            _context = context;
        }
        public IActionResult RequestForm(string songRequest,string email, string bandName, string bandLocation)
        {
            
            //validify all input
            if(HttpContext.Session.GetString("captcha") != "verified" || String.IsNullOrEmpty(HttpContext.Session.GetString("captcha")))
                return BadRequest("Please verify that you are human");
            if(!Functions.Validify.IsValidEmail(email))
                return BadRequest("Invalid email");
            if(String.IsNullOrEmpty(songRequest) || String.IsNullOrEmpty(bandName) || String.IsNullOrEmpty(bandLocation))
                return BadRequest("Please fill up all form fields");

            using(_context)
            {
                _context.Database.EnsureCreated();

                //Log
                log = new Log();
                Visitor visitor = log.Initialize(_context,_accessor);
                log.LogContent("Index", "Sending form");
                _context.Log.Add(log);

                //Check if band exists if not, create one
                Band band = _context.Band.Where(v => v.BandName == bandName).FirstOrDefault() ?? new Band();
                if(String.IsNullOrEmpty(band.BandName))
                {
                    band = new Band();
                    band.BandName = bandName;
                    band.BandLocation = bandLocation;
                    _context.Band.Add(band);
                }

                //Create new song request
                SongRequest newSongRequest = new SongRequest{
                    SongRequest1 = songRequest,
                    IdVisitorNavigation = visitor,
                    Email = email
                };

                _context.SongRequest.Add(newSongRequest);

                Functions.Mail.SendMail("New song request",stringBuilder.Append("Band: ").Append(bandName).Append(" - From: ").Append(bandLocation).Append("<br>Message:<br>").Append(songRequest).ToString(),email);
                _context.SaveChanges();
            }
            return Ok("Thank you!");
        }
        public StatusCodeResult ReCaptchaVerify(string captchaResponse)
        {
            IRestResponse reCaptcha;
            using(_context)
            {
                _context.Database.EnsureCreated();

                //log
                log = new Log();
                Visitor visitor = log.Initialize(_context,_accessor);
                log.LogContent("g-recaptchaSend", "Sending captcha for verification");
                _context.Log.Add(log);
                _context.SaveChanges();

                //reCaptcha verify
                RequestFactory RC = new RequestFactory();
                RestRequest restRequest = RC.RequestConstructor(
                    new Dictionary<string,string>()
                        {
                            {"secret",secretKey},
                            {"response", captchaResponse},
                            {"remoteip", _accessor.HttpContext.Connection.RemoteIpAddress.ToString()}
                        },
                        Method.POST);
                reCaptcha = RC.RequestSender(url,restRequest);

                //log
                log = new Log();
                log.Initialize(_context,_accessor);
                log.LogContent("g-recaptchaResponse", "Receiving captcha response");
                _context.Log.Add(log);
                
                _context.SaveChanges();
            }

            //Check if captcha was successful or not
            //if(reCaptcha.Content.Contains('"'+"success"+'"'+": true"))
            if(reCaptcha.Content.Contains("true"))
            {
                HttpContext.Session.SetString("captcha","verified");
                return Ok();
            }
            return BadRequest();
        }
    }
}
