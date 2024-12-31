using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly BlogsContext _context;

        public HomeController(ILogger<HomeController> logger,BlogsContext context)
		{
			_logger = logger;
			_context = context;
		}

        public  IActionResult Index()
        {
			ViewData["Username"] = HttpContext.Session.GetString("Username");

			return View();
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
