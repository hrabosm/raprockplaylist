using System;
using System.Diagnostics;
using System.Linq;
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
        [Route("error/{code:int}")]
        public IActionResult ErrorCode(int code)
        {
            Error err = new Error();
            err.errorNumber = code;
            ViewBag.Message = err;
            return View("error");
        }
        public IActionResult Index()
        {
            using(_context)
            {
                log = new Log();
                Visitor visitor = log.Initialize(_context,_accessor);
                log.LogContent("Index", "Loading page");
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            using(_context)
            {
                log = new Log();
                Visitor visitor = log.Initialize(_context,_accessor);
                log.LogContent("Privacy", "Loading page");
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            return View();
        }
        public IActionResult Success()
        {
            using(_context)
            {
                log = new Log();
                Visitor visitor = log.Initialize(_context,_accessor);
                log.LogContent("Index", "Loaded main page");
                _context.Log.Add(log);
                _context.SaveChanges();
            }
            return View();
        }
        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
