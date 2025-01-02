using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Blog.Filters;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


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
        public async Task<IActionResult> Index(string searchQuery)
        {
            var userId = HttpContext.Session.GetString("UserId");
            ViewData["UserId"] = userId;

            var blogPostsQuery = _context.BlogPosts
                .Include(bp => bp.User)
                .Include(bp => bp.Comments)
                .ThenInclude(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                blogPostsQuery = blogPostsQuery.Where(bp => bp.Title.Contains(searchQuery));
            }

            var blogPosts = await blogPostsQuery.ToListAsync();

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

            blogPost.UserId = userId;
            blogPost.CreatedAt = DateTime.Now;
            blogPost.UpdatedAt = DateTime.Now;

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [RequireLogin]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
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

            var existingPost = await _context.BlogPosts.FindAsync(id);
            if (existingPost == null || existingPost.UserId != userId)
            {
                return Forbid();
            }

            existingPost.Title = blogPost.Title;
            existingPost.Content = blogPost.Content;
            existingPost.UpdatedAt = DateTime.Now;

            _context.Update(existingPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [RequireLogin]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(bp => bp.User)
                .FirstOrDefaultAsync(bp => bp.Id == id);

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
            var blogPost = await _context.BlogPosts.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId) || blogPost.UserId != userId)
            {
                return Forbid();
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [RequireLogin]
        [HttpGet]
        public  IActionResult CreateComment(int blogPostId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Forbid();
            }

            
            var blogPost =  _context.BlogPosts.Find(blogPostId);
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
                var blogPost = await _context.BlogPosts.FindAsync(model.BlogPostId);
                if (blogPost == null)
                {
                    ModelState.AddModelError("", "Blog post not found.");
                    return View(model);
                }

                
                var comment = new Comment
                {
                    Content = model.Content, 
                    BlogPostId = blogPost.Id,
                    UserId = model.UserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Comments.Add(comment); 
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }




    }
}

