using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entities.Enums;
using ExpenseTracker.App.Services;
using System.Collections.ObjectModel;

namespace ExpenseTracker.App.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;

    [ObservableProperty]
    private string userEmail = string.Empty;

    [ObservableProperty]
    private bool isLoggedIn = false;

    [ObservableProperty]
    private string selectedTheme = "Light";

    public ObservableCollection<string> AvailableThemes { get; } = new()
    {
        "System",
        "Light",
        "Dark"
    };

    public SettingsViewModel(IPreferencesService preferencesService, IThemeService themeService)
    {
        _preferencesService = preferencesService;
        _themeService = themeService;
        LoadUserInfo();
        LoadThemeInfo();
    }

    private void LoadUserInfo()
    {
        IsLoggedIn = _preferencesService.IsLoggedIn;
        UserEmail = _preferencesService.UserEmail;
    }

    private void LoadThemeInfo()
    {
        SelectedTheme = _themeService.CurrentTheme.ToString();
    }

    partial void OnSelectedThemeChanged(string value)
    {
        if (Enum.TryParse<AppThemes>(value, out var theme))
        {
            _themeService.SetTheme(theme);
        }
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