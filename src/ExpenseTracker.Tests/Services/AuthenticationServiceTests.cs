using Xunit;
using ExpenseTracker.Infrastructure.Services;
using ExpenseTracker.Infrastructure.Interfaces;

namespace ExpenseTracker.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly IAuthenticationService _authService;

    public AuthenticationServiceTests()
    {
        _authService = new DemoAuthenticationService();
    }

    [Theory]
    [InlineData("demo@demo.com", "123456")]
    [InlineData("mostafa@tefa.com", "123456")]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnUser(string email, string password)
    {
        // Act
        var result = await _authService.LoginAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Theory]
    [InlineData("demo@demo.com", "wrongpassword")]
    [InlineData("wrong@email.com", "123456")]
    [InlineData("", "123456")]
    [InlineData("demo@demo.com", "")]
    public async Task LoginAsync_WithInvalidCredentials_ShouldReturnNull(string email, string password)
    {
        // Act
        var result = await _authService.LoginAsync(email, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithNullEmail_ShouldReturnNull()
    {
        // Act
        var result = await _authService.LoginAsync(null!, "123456");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithNullPassword_ShouldReturnNull()
    {
        // Act
        var result = await _authService.LoginAsync("demo@demo.com", null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithWhitespaceCredentials_ShouldReturnNull()
    {
        // Act
        var result = await _authService.LoginAsync("   ", "   ");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_IsCaseSensitive_ShouldReturnNullForWrongCase()
    {
        // Act - The service is case sensitive, so wrong case should fail
        var result1 = await _authService.LoginAsync("DEMO@DEMO.COM", "123456");
        var result2 = await _authService.LoginAsync("demo@DEMO.com", "123456");

        // Assert
        Assert.Null(result1); // Case sensitive, should fail
        Assert.Null(result2); // Case sensitive, should fail
    }
}