using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly BlogsContext _context;

        public AccountController(ILogger<AccountController> logger, BlogsContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username", "Password", "Email")] User user)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {

                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(user);
                }


                await _context.AddAsync(user);
                await _context.SaveChangesAsync();


                return RedirectToAction("Login", "Account");
            }


            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username", "Password")] User model)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);


            if (user != null)
            {

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("UserId", user.Id);



                return RedirectToAction("Index", "Home");
            }
            else
            {

                ModelState.AddModelError("", "Invalid username or password.");
            }


            return View(model);



        }
        public  IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }

}
