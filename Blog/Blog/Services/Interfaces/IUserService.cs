using Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IUserService
    {

        Task<List<User>> GetUsersAsync();


        Task<bool> CreateUserAsync(User model);


        Task<User> GetUserByIdAsync(int id);


        Task<bool> UpdateUserAsync(User model);


        Task<bool> DeleteUserAsync(int id);
    }
}
