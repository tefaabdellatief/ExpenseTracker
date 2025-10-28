using Xunit;
using Moq;
using ExpenseTracker.Infrastructure.Interfaces;
using Entities.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace ExpenseTracker.Tests.ViewModels;

// Test interfaces to avoid MAUI dependencies
public interface IPreferencesService
{
    bool IsLoggedIn { get; set; }
    string UserEmail { get; set; }
}

public interface IDialogService
{
    Task ShowErrorAsync(string title, string message);
}

// Testable LoginViewModel implementation
public partial class TestableLoginViewModel : ObservableObject
{
    private readonly IAuthenticationService _authService;
    private readonly IPreferencesService _preferencesService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isPasswordVisible = false;

    [ObservableProperty] 
    private bool isBusy;
    
    [ObservableProperty] 
    private string? errorMessage;

    public TestableLoginViewModel(IAuthenticationService authService, IPreferencesService preferencesService, IDialogService dialogService)
    {
        _authService = authService;
        _preferencesService = preferencesService;
        _dialogService = dialogService;
    }

    [RelayCommand]
    public void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await _dialogService.ShowErrorAsync("Validation Error", "Email is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            await _dialogService.ShowErrorAsync("Validation Error", "Password is required.");
            return;
        }

        if (!IsValidEmail(Email))
        {
            await _dialogService.ShowErrorAsync("Validation Error", "Please enter a valid email address.");
            return;
        }

        await RunSafeAsync(async () =>
        {
            var user = await _authService.LoginAsync(Email, Password);
            
            if (user != null)
            {
                _preferencesService.IsLoggedIn = true;
                _preferencesService.UserEmail = user.Email;
            }
            else
            {
                await _dialogService.ShowErrorAsync("Login Failed", "Invalid email or password. Please try again.");
            }
        });
    }

    [RelayCommand]
    public async Task ContinueAsGuestAsync()
    {
        _preferencesService.IsLoggedIn = false;
        _preferencesService.UserEmail = "Guest";
        await Task.CompletedTask;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected async Task RunSafeAsync(Func<Task> action)
    {
        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public class LoginViewModelTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly Mock<IPreferencesService> _mockPreferencesService;
    private readonly Mock<IDialogService> _mockDialogService;
    private readonly TestableLoginViewModel _viewModel;

    public LoginViewModelTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _mockPreferencesService = new Mock<IPreferencesService>();
        _mockDialogService = new Mock<IDialogService>();
        
        _viewModel = new TestableLoginViewModel(
            _mockAuthService.Object,
            _mockPreferencesService.Object,
            _mockDialogService.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Assert
        Assert.Equal(string.Empty, _viewModel.Email);
        Assert.Equal(string.Empty, _viewModel.Password);
        Assert.False(_viewModel.IsPasswordVisible);
        Assert.False(_viewModel.IsBusy);
    }

    [Fact]
    public void TogglePasswordVisibility_ShouldToggleVisibility()
    {
        // Arrange
        var initialVisibility = _viewModel.IsPasswordVisible;

        // Act
        _viewModel.TogglePasswordVisibilityCommand.Execute(null);

        // Assert
        Assert.Equal(!initialVisibility, _viewModel.IsPasswordVisible);
    }

    [Fact]
    public void TogglePasswordVisibility_ShouldToggleMultipleTimes()
    {
        // Act & Assert
        Assert.False(_viewModel.IsPasswordVisible);
        
        _viewModel.TogglePasswordVisibilityCommand.Execute(null);
        Assert.True(_viewModel.IsPasswordVisible);
        
        _viewModel.TogglePasswordVisibilityCommand.Execute(null);
        Assert.False(_viewModel.IsPasswordVisible);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyEmail_ShouldShowValidationError()
    {
        // Arrange
        _viewModel.Email = "";
        _viewModel.Password = "password123";

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockDialogService.Verify(x => x.ShowErrorAsync("Validation Error", "Email is required."), Times.Once);
        _mockAuthService.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithWhitespaceEmail_ShouldShowValidationError()
    {
        // Arrange
        _viewModel.Email = "   ";
        _viewModel.Password = "password123";

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockDialogService.Verify(x => x.ShowErrorAsync("Validation Error", "Email is required."), Times.Once);
        _mockAuthService.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyPassword_ShouldShowValidationError()
    {
        // Arrange
        _viewModel.Email = "test@example.com";
        _viewModel.Password = "";

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockDialogService.Verify(x => x.ShowErrorAsync("Validation Error", "Password is required."), Times.Once);
        _mockAuthService.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithWhitespacePassword_ShouldShowValidationError()
    {
        // Arrange
        _viewModel.Email = "test@example.com";
        _viewModel.Password = "   ";

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockDialogService.Verify(x => x.ShowErrorAsync("Validation Error", "Password is required."), Times.Once);
        _mockAuthService.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test.example.com")]
    [InlineData("test@.com")]
    public async Task LoginAsync_WithInvalidEmail_ShouldShowValidationError(string invalidEmail)
    {
        // Arrange
        _viewModel.Email = invalidEmail;
        _viewModel.Password = "password123";

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockDialogService.Verify(x => x.ShowErrorAsync("Validation Error", "Please enter a valid email address."), Times.Once);
        _mockAuthService.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("test123@gmail.com")]
    public async Task LoginAsync_WithValidEmailAndSuccessfulAuth_ShouldLoginSuccessfully(string validEmail)
    {
        // Arrange
        var testUser = new UserDto { Email = validEmail };
        _viewModel.Email = validEmail;
        _viewModel.Password = "password123";

        _mockAuthService.Setup(x => x.LoginAsync(validEmail, "password123"))
                       .ReturnsAsync(testUser);

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockAuthService.Verify(x => x.LoginAsync(validEmail, "password123"), Times.Once);
        _mockPreferencesService.VerifySet(x => x.IsLoggedIn = true, Times.Once);
        _mockPreferencesService.VerifySet(x => x.UserEmail = validEmail, Times.Once);
        _mockDialogService.Verify(x => x.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentialsButFailedAuth_ShouldShowLoginError()
    {
        // Arrange
        _viewModel.Email = "test@example.com";
        _viewModel.Password = "wrongpassword";

        _mockAuthService.Setup(x => x.LoginAsync("test@example.com", "wrongpassword"))
                       .ReturnsAsync((UserDto?)null);

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        _mockAuthService.Verify(x => x.LoginAsync("test@example.com", "wrongpassword"), Times.Once);
        _mockDialogService.Verify(x => x.ShowErrorAsync("Login Failed", "Invalid email or password. Please try again."), Times.Once);
        _mockPreferencesService.VerifySet(x => x.IsLoggedIn = It.IsAny<bool>(), Times.Never);
        _mockPreferencesService.VerifySet(x => x.UserEmail = It.IsAny<string>(), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenAuthServiceThrowsException_ShouldHandleError()
    {
        // Arrange
        _viewModel.Email = "test@example.com";
        _viewModel.Password = "password123";

        _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ThrowsAsync(new Exception("Network error"));

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        Assert.Equal("Network error", _viewModel.ErrorMessage);
        Assert.False(_viewModel.IsBusy);
        _mockPreferencesService.VerifySet(x => x.IsLoggedIn = It.IsAny<bool>(), Times.Never);
    }

    [Fact]
    public async Task ContinueAsGuestAsync_ShouldSetGuestMode()
    {
        // Act
        await _viewModel.ContinueAsGuestCommand.ExecuteAsync(null);

        // Assert
        _mockPreferencesService.VerifySet(x => x.IsLoggedIn = false, Times.Once);
        _mockPreferencesService.VerifySet(x => x.UserEmail = "Guest", Times.Once);
    }

    [Fact]
    public void Email_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.Email))
                propertyChangedRaised = true;
        };

        // Act
        _viewModel.Email = "test@example.com";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("test@example.com", _viewModel.Email);
    }

    [Fact]
    public void Password_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.Password))
                propertyChangedRaised = true;
        };

        // Act
        _viewModel.Password = "newpassword";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("newpassword", _viewModel.Password);
    }

    [Fact]
    public void IsPasswordVisible_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.IsPasswordVisible))
                propertyChangedRaised = true;
        };

        // Act
        _viewModel.IsPasswordVisible = true;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.True(_viewModel.IsPasswordVisible);
    }

    [Fact]
    public void IsBusy_PropertyChanged_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.IsBusy))
                propertyChangedRaised = true;
        };

        // Act
        _viewModel.IsBusy = true;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.True(_viewModel.IsBusy);
    }

    [Fact]
    public async Task LoginAsync_ShouldSetIsBusyDuringExecution()
    {
        // Arrange
        var testUser = new UserDto { Email = "test@example.com" };
        _viewModel.Email = "test@example.com";
        _viewModel.Password = "password123";

        var busyStates = new List<bool>();
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.IsBusy))
                busyStates.Add(_viewModel.IsBusy);
        };

        _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(testUser);

        // Act
        await _viewModel.LoginCommand.ExecuteAsync(null);

        // Assert
        Assert.Contains(true, busyStates); // Should have been set to ttrue during execution
        Assert.False(_viewModel.IsBusy); // Should be false after completion
    }
}