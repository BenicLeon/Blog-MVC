using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Filters;
using Blog.Models;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly BlogsContext _context;

        public UserController(ILogger<UserController> logger, BlogsContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        [RequireLogin]
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> Create([Bind("Username", "Email", "Password")] User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }


            return View(model);
        }
        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetUserAsync(id);
            return View(user);
        }
        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> Edit(int id, [Bind("Username", "Password", "Email")] User model)
        {
            if (ModelState.IsValid)
            {
				var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }
                else
                {
                    _context.Users.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await GetUserAsync(id);
            return View(user);
            
        }
        [HttpPost, ActionName("Delete")]
        [RequireLogin]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        private async Task<User> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
       
    }
}

