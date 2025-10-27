using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.App.Services;
using ExpenseTracker.Infrastructure.Interfaces;

namespace ExpenseTracker.App.ViewModels;

public partial class LoginViewModel : BaseViewModel
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

    public LoginViewModel(IAuthenticationService authService, IPreferencesService preferencesService, IDialogService dialogService)
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
                // Save login state
                _preferencesService.IsLoggedIn = true;
                _preferencesService.UserEmail = user.Email;
                
                await Toast.Make("Login successful!").Show();
                
                // Navigate to main app
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//main");
                }
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
        
        await Toast.Make("Continuing as guest").Show();
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//main");
        }
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


}