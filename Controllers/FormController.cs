using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RaprockPlaylist.Context;
using RaprockPlaylist.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace RaprockPlaylist.Controllers
{
    
    public class FormController : Controller
    {
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
    }
}
