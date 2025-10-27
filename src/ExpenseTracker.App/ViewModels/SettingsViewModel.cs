using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.App.Services;

namespace ExpenseTracker.App.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IPreferencesService _preferencesService;

    [ObservableProperty]
    private string userEmail = string.Empty;

    [ObservableProperty]
    private bool isLoggedIn = false;

    [ObservableProperty]
    private string selectedTheme = "System";

    public SettingsViewModel(IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        IsLoggedIn = _preferencesService.IsLoggedIn;
        UserEmail = _preferencesService.UserEmail;
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        if (Shell.Current == null) return;
        
        var confirm = await Shell.Current.DisplayAlert(
            "Logout", 
            "Are you sure you want to logout?", 
            "Yes", 
            "No");
            
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            _preferencesService.ClearUserData();
            await Toast.Make("Logged out successfully").Show();
            
            // Navigate to login and clear navigation stack
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//login");
            }
        });
    }

    [RelayCommand]
    public async Task ShowAboutAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlert("About Expense Tracker",
                "Version 1.0.0",
                "OK");
        }
    }
}