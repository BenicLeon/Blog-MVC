using Blog.Filters;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

       
        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsersAsync();
            return View(users);
        }

        
        [HttpGet]
        [RequireLogin]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> Create([Bind("Username", "Email", "Password")] User model)
        {
            if (ModelState.IsValid)
            {
                bool success = await _userService.CreateUserAsync(model);
                if (!success)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        
        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> Edit(int id, [Bind("Username", "Password", "Email")] User model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;  

                bool success = await _userService.UpdateUserAsync(model);
                if (!success)
                {
                    ModelState.AddModelError("Email", "This email is already registered or in use by another user.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        [RequireLogin]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        
        [HttpPost, ActionName("Delete")]
        [RequireLogin]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}
