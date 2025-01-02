using Blog.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly BlogsContext _context;
        private readonly ILogger<BlogService> _logger;

        public BlogService(BlogsContext context, ILogger<BlogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<List<BlogPost>> GetBlogPostsAsync(string searchQuery)
        {
            var blogPostsQuery = _context.BlogPosts
                .Include(bp => bp.User)
                .Include(bp => bp.Comments)
                .ThenInclude(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                blogPostsQuery = blogPostsQuery.Where(bp => bp.Title.Contains(searchQuery));
            }

            return await blogPostsQuery.ToListAsync();
        }

        
        public async Task<BlogPost> GetBlogPostByIdAsync(int id)
        {
            return await _context.BlogPosts
                .Include(bp => bp.User)
                .Include(bp => bp.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(bp => bp.Id == id);
        }

        
        public async Task<bool> CreateBlogPostAsync(BlogPost blogPost, int userId)
        {
            try
            {
                blogPost.UserId = userId;
                blogPost.CreatedAt = DateTime.Now;
                blogPost.UpdatedAt = DateTime.Now;

                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                return false;
            }
        }

        
        public async Task<bool> UpdateBlogPostAsync(BlogPost blogPost, int userId)
        {
            try
            {
                var existingPost = await _context.BlogPosts.FindAsync(blogPost.Id);
                if (existingPost == null || existingPost.UserId != userId)
                {
                    return false;
                }

                existingPost.Title = blogPost.Title;
                existingPost.Content = blogPost.Content;
                existingPost.UpdatedAt = DateTime.Now;

                _context.Update(existingPost);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post");
                return false;
            }
        }

        
        public async Task<bool> DeleteBlogPostAsync(int id, int userId)
        {
            try
            {
                var blogPost = await _context.BlogPosts.FindAsync(id);
                if (blogPost == null || blogPost.UserId != userId)
                {
                    return false;
                }

                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog post");
                return false;
            }
        }

        
        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            try
            {
                comment.CreatedAt = DateTime.Now;
                comment.UpdatedAt = DateTime.Now;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment");
                return false;
            }
        }
    }
}
