using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Blog.Services.Interfaces;

namespace Blog.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly BlogsContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(BlogsContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<bool> CreateUserAsync(User model)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    return false;
                }

                await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<bool> UpdateUserAsync(User model)
        {
            try
            {
                
                if (model.Email != null)
                {
                    var existingUserWithNewEmail = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == model.Email && u.Id != model.Id);

                   
                    if (existingUserWithNewEmail != null)
                    {
                        return false; 
                    }
                }

                
                _context.Users.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return false;
            }
        }

        
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return false;
            }
        }
    }
}
