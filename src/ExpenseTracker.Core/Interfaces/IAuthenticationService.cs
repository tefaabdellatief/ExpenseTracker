using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IAuthenticationService
{
    Task<User?> LoginAsync(string email, string password);
}