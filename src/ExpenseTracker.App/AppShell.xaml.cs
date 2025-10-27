using Microsoft.Maui.Controls;
using ExpenseTracker.App.Views;

namespace ExpenseTracker.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute(nameof(AddEditExpensePage), typeof(AddEditExpensePage));
    }
}