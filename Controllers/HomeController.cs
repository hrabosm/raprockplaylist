using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaprockPlaylist.Models;
using RaprockPlaylist.Context;
using RaprockPlaylist.Functions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http.Headers;

namespace RaprockPlaylist.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<HomeController> _logger;
        private readonly PlaylistContext _context;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor accessor, PlaylistContext context)
        {
            _logger = logger;
            _accessor = accessor;
            _context = context;
        }
        public IActionResult Index()
        {
            _context.Database.EnsureCreated();
            Functions.Log.LogActivity(_context,"Index", "Loaded main page",_accessor);
            _context.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            _context.Database.EnsureCreated();
            Functions.Log.LogActivity(_context,"Privacy", "Loading page",_accessor);
            _context.SaveChanges();
            return View();
        }
        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            _context.Database.EnsureCreated();
            Functions.Log.LogActivity(_context,"Not Found",HttpContext.Request.Path,_accessor);
            Response.StatusCode = 404;
            ViewData["errorCode"] = 404;
            return View("Error");
        }
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            _context.Database.EnsureCreated();
            Functions.Log.LogError(_context,HttpContext.Request.Path, "User encountered error: "+code,_accessor);
            _context.SaveChanges();
            ViewData["errorCode"] = code;
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
