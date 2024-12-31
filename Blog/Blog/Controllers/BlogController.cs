using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Blog.Filters;
using System.Diagnostics;


namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogsContext _context;

        public BlogController(ILogger<HomeController> logger, BlogsContext context)
        {
            _logger = logger;
            _context = context;
        }

        [RequireLogin]
        public IActionResult Blog()
        {


			var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;


			return View();
        }
        public IActionResult Logout() 
        { 
            HttpContext.Session.Clear();    
            return RedirectToAction("Login","Login");
        }

        
    }
}
