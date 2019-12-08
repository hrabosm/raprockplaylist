using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaprockPlaylist.Models;
using RaprockPlaylist.Context;
using Microsoft.EntityFrameworkCore.Internal;

namespace RaprockPlaylist.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<HomeController> _logger;
        private readonly PlaylistContext _context;
        private Log log;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor accessor, PlaylistContext context)
        {
            _logger = logger;
            _accessor = accessor;
            _context = context;
        }

        public IActionResult Index()
        {
            using(_context)
            {
                Visitor visitor = _context.Visitor.Where(v => v.IpAdress == _accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOr(new Visitor());
                if(String.IsNullOrEmpty(visitor.IpAdress))
                {
                    visitor.GetIpAdress(_accessor);
                    _context.Visitor.Add(visitor);
                }
                log = new Log();
                log.IdVisitorNavigation = visitor;
                log.Source = "Index";
                log.Message = "Loading page";
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            using(_context)
            {
                Visitor visitor = _context.Visitor.Where(v => v.IpAdress == _accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOr(new Visitor());
                if(String.IsNullOrEmpty(visitor.IpAdress))
                {
                    visitor.GetIpAdress(_accessor);
                    _context.Visitor.Add(visitor);
                }
                log = new Log();
                log.IdVisitorNavigation = visitor;
                log.Source = "Privacy";
                log.Message = "Loading page";
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            return View();
        }
        public IActionResult Success()
        {
            using(_context)
            {
                Visitor visitor = _context.Visitor.Where(v => v.IpAdress == _accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOr(new Visitor());
                if(String.IsNullOrEmpty(visitor.IpAdress))
                {
                    visitor.GetIpAdress(_accessor);
                    _context.Visitor.Add(visitor);
                }
                log = new Log();
                log.IdVisitorNavigation = visitor;
                log.Source = "Index";
                log.Message = "Loaded main page";
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            TempData.Remove("idNewRequest");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
