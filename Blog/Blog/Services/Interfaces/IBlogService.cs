using Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPostsAsync(string searchQuery);
        Task<BlogPost> GetBlogPostByIdAsync(int id);
        Task<bool> CreateBlogPostAsync(BlogPost blogPost, int userId);
        Task<bool> UpdateBlogPostAsync(BlogPost blogPost, int userId);
        Task<bool> DeleteBlogPostAsync(int id, int userId);
        Task<bool> CreateCommentAsync(Comment comment);
    }
}
