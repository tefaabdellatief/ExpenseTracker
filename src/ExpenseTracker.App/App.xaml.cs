using Microsoft.Maui.Controls;
using ExpenseTracker.App.Views;
using ExpenseTracker.App.Services;

namespace ExpenseTracker.App;

public partial class App : Application
{
    private readonly IPreferencesService _preferencesService;

    public App(IPreferencesService preferencesService)
    {
        InitializeComponent();
        _preferencesService = preferencesService;
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            
            if (_preferencesService.IsLoggedIn)
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