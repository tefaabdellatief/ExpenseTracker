using Microsoft.Maui.Controls;
using ExpenseTracker.App.Views;
using ExpenseTracker.App.Services;

namespace ExpenseTracker.App;

public partial class App : Application
{
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;

    public App(IPreferencesService preferencesService, IThemeService themeService)
    {
        InitializeComponent();
        _preferencesService = preferencesService;
        _themeService = themeService;
        
        MainPage = new AppShell();

        _themeService.ApplyTheme();

    }

    protected override void OnStart()
    {
        base.OnStart();
        
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            
            if (_preferencesService != null && _preferencesService.IsLoggedIn)
            {
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                await Shell.Current.GoToAsync("//login");
            }
        });
    }
}