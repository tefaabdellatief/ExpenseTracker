using Entities.Dtos;
using ExpenseTracker.Infrastructure.Interfaces;

namespace ExpenseTracker.Infrastructure.Services;

public class DemoAuthenticationService : IAuthenticationService
{
    private readonly List<UserDto> _users = new() { 
        new UserDto { Email = "demo@demo.com", Password = "123456" },
        new UserDto { Email = "mostafa@tefa.com", Password = "123456" }
    };

    public async Task<UserDto?> LoginAsync(string email, string password)
    {
        await Task.Delay(200);
        return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}