using Entities.Dtos;

namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IAuthenticationService
{
    public Task<UserDto?> LoginAsync(string email, string password);
}