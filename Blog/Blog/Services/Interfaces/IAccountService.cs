using Blog.Models;
public interface IAccountService
{
    Task<bool> RegisterUserAsync(User user);
    Task<User> AuthenticateUserAsync(string username, string password);
    Task<User> GetUserByEmailAsync(string email);
}