using Blog.Data;
using Blog.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly BlogsContext _context;
        private readonly ILogger<AccountService> _logger;

        public AccountService(BlogsContext context, ILogger<AccountService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {

                var existingUser = await GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return false;
                }


                user.Password = HashPassword(user.Password);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return false;
            }
        }


        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }


        public async Task RehashPasswords()
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {

                if (!user.Password.StartsWith("$2a$") && !user.Password.StartsWith("$2b$") && !user.Password.StartsWith("$2y$"))
                {

                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _context.Update(user);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
