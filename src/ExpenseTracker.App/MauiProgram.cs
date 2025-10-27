using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Infrastructure.Services;
using ExpenseTracker.Infrastructure.Data;
using ExpenseTracker.App.ViewModels;
using ExpenseTracker.App.Views;
using System.IO;
using CommunityToolkit.Maui;

namespace ExpenseTracker.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(f => f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        // DB path
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db3");

        // Register infrastructure
        builder.Services.AddSingleton<IExpenseService, MockExpenseService>();
        builder.Services.AddSingleton<IAuthenticationService, DemoAuthenticationService>();
        builder.Services.AddSingleton<IDataService>(sp => new DatabaseService(dbPath));
        builder.Services.AddSingleton<DataSyncService>();

        // ViewModels & Views
        builder.Services.AddTransient<ExpenseListViewModel>();
        builder.Services.AddTransient<AddEditExpenseViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddSingleton<Views.ExpenseListPage>();
        builder.Services.AddTransient<Views.AddEditExpensePage>();
        builder.Services.AddSingleton<Views.DashboardPage>();
        builder.Services.AddSingleton<Views.SettingsPage>();

        var app = builder.Build();

        // Initialize DB and seed
        var dataService = app.Services.GetRequiredService<IDataService>() as DatabaseService;
        Task.Run(async () => {
            await dataService.InitializeAsync();
            await DatabaseInitializer.SeedAsync(dataService.Connection);
        }).Wait();

        return app;
    }
}
