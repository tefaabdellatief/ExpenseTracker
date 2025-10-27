using Microsoft.Maui.Controls;
using ExpenseTracker.App.Views;

namespace ExpenseTracker.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}