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
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            Functions.Log.LogError(_context,HttpContext.Request.Path, "User encountered error: "+code,_accessor);
            _context.SaveChanges();
            ViewData["errorCode"] = code;
            return View();
        }
        public IActionResult Index()
        {
            Functions.Log.LogActivity(_context,"Index", "Loaded main page",_accessor);
            _context.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            Functions.Log.LogActivity(_context,"Privacy", "Loading page",_accessor);
            _context.SaveChanges();
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
