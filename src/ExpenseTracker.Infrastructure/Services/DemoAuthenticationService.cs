using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Infrastructure.Services;

public class DemoAuthenticationService : IAuthenticationService
{
    private readonly List<User> _users = new() { new User { Email = "demo@demo.com", Password = "123456" } };

    public async Task<User?> LoginAsync(string email, string password)
    {
        await Task.Delay(200);
        return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}
