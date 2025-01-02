using Blog.Filters;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
           
        }

        [RequireLogin]
        public async Task<IActionResult> Index(string searchQuery)
        {
            var userId = HttpContext.Session.GetString("UserId");
            ViewData["UserId"] = userId;

            var blogPosts = await _blogService.GetBlogPostsAsync(searchQuery);

            if (!blogPosts.Any())
            {
                ViewBag.Message = "No blog posts found for this search.";
            }

            return View(blogPosts);
        }

        [RequireLogin]
        public IActionResult Create()
        {
            return View();
        }

        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return View(blogPost);
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                ModelState.AddModelError("", "User not logged in or invalid session.");
                return View(blogPost);
            }

            var success = await _blogService.CreateBlogPostAsync(blogPost, userId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error creating blog post.");
            return View(blogPost);
        }

        [RequireLogin]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogService.GetBlogPostByIdAsync(id.Value);
            if (blogPost == null)
            {
                return NotFound();
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId) || blogPost.UserId != userId)
            {
                return Forbid();
            }

            return View(blogPost);
        }

        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(blogPost);
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Forbid();
            }

            var success = await _blogService.UpdateBlogPostAsync(blogPost, userId);
            if (success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error updating blog post.");
            return View(blogPost);
        }

        [RequireLogin]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogService.GetBlogPostByIdAsync(id.Value);
            if (blogPost == null)
            {
                return NotFound();
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId) || blogPost.UserId != userId)
            {
                return Forbid();
            }

            return View(blogPost);
        }

        [RequireLogin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Forbid();
            }

            var success = await _blogService.DeleteBlogPostAsync(id, userId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            return Forbid();
        }

        [RequireLogin]
        [HttpGet]
        public IActionResult CreateComment(int blogPostId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Forbid();
            }

            var blogPost = _blogService.GetBlogPostByIdAsync(blogPostId).Result;
            if (blogPost == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                BlogPostId = blogPost.Id,
                UserId = userId
            };

            return View(comment);
        }

        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(Comment model)
        {
            if (ModelState.IsValid)
            {
                var success = await _blogService.CreateCommentAsync(model);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error creating comment.");
            }

            return View(model);
        }
    }
}
