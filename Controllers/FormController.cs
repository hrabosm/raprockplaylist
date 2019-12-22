using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaprockPlaylist.Context;
using RaprockPlaylist.Models;
using System;
using System.Web;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using RestSharp;
using System.Collections.Generic;
using RaprockPlaylist.Factories;

namespace RaprockPlaylist.Controllers
{
    
    public class FormController : Controller
    {
        private string url = "https://www.google.com/recaptcha/api/siteverify";
        private string secretKey = "6LdB_8gUAAAAAOjR8cm667pv5oJLN_mh7soFxuKH";
        private readonly IHttpContextAccessor _accessor;
        private readonly PlaylistContext _context;
        private Log log;

        public FormController(IHttpContextAccessor accessor,PlaylistContext context)
        {
            _accessor = accessor;
            _context = context;
        }
        public IActionResult RequestForm(string songRequest,string email, string bandName, string bandLocation)
        {
            using(_context)
            {
                if(HttpContext.Session.GetString("captcha") != "verified" || String.IsNullOrEmpty(HttpContext.Session.GetString("captcha")))
                    return BadRequest();

                _context.Database.EnsureCreated();
                Visitor visitor = _context.Visitor.Where(v => v.IpAdress == _accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOr(new Visitor());
                if(String.IsNullOrEmpty(visitor.IpAdress))
                {
                    visitor.GetIpAdress(_accessor);
                    _context.Visitor.Add(visitor);
                }
                log = new Log();
                log.IdVisitorNavigation = visitor;
                log.Source = "Index";
                log.Message = "Sending form";
                _context.Log.Add(log);
                
                Band band = _context.Band.Where(v => v.BandName == bandName).FirstOr(new Band());
                if(String.IsNullOrEmpty(band.BandName))
                {
                    band.BandName = bandName;
                    band.BandLocation = bandLocation;
                    _context.Band.Add(band);
                }

                var newSongRequest = new SongRequest{
                    SongRequest1 = songRequest,
                    IdVisitorNavigation = visitor,
                    Email = email
                };
                _context.SongRequest.Add(newSongRequest);
                TempData.Remove("idNewRequest");
                TempData.Add("idNewRequest",newSongRequest.IdSongRequest);
                _context.SaveChanges();
            }
            return RedirectToAction("Success","Home");
        }
        public StatusCodeResult ReCaptchaVerify(string captchaResponse)
        {
            IRestResponse reCaptcha;
            using(_context)
            {
                _context.Database.EnsureCreated();
                
                Visitor visitor = _context.Visitor.Where(v => v.IpAdress == _accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOr(new Visitor());
                if(String.IsNullOrEmpty(visitor.IpAdress))
                {
                    visitor.GetIpAdress(_accessor);
                    _context.Visitor.Add(visitor);
                }
                log = new Log();
                log.Source = "g-recaptchaSend";
                log.IdVisitorNavigation = visitor;
                log.Message = "Sending captcha for verification";
                _context.Log.Add(log);
                _context.SaveChanges();
                RequestFactory RC = new RequestFactory();
                RestRequest restRequest = RC.RequestConstructor(new Dictionary<string,string>()
                                            {
                                                {"secret",secretKey},
                                                {"response", captchaResponse},
                                                {"remoteip", _accessor.HttpContext.Connection.RemoteIpAddress.ToString()}
                                            },Method.POST);
                reCaptcha = RC.RequestSender(url,restRequest);
                log = new Log();
                log.Source = "g-recaptchaResponse";
                log.IdVisitorNavigation = visitor;
                log.Message = "Recieving captcha response";
                _context.Log.Add(log);
                _context.SaveChanges();

            }
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
