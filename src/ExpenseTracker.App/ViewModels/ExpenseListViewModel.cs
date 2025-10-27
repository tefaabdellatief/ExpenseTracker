using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using System.Collections.ObjectModel;

namespace ExpenseTracker.App.ViewModels;

public partial class ExpenseListViewModel : BaseViewModel
{
    private readonly IExpenseService _service;

    public ObservableCollection<Expense> Expenses { get; } = new();
    [ObservableProperty] private string searchText = string.Empty;

    public ExpenseListViewModel(IExpenseService service) => _service = service;

    [RelayCommand]
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Expenses.Clear();
            var items = await _service.GetAllAsync();
            foreach (var e in items)
                Expenses.Add(e);
        });
    }

    [RelayCommand]
    public async Task NavigateAddAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddEditExpensePage));
    }

    [RelayCommand]
    public async Task DeleteAsync(Expense e)
    {
        if (e == null) return;
        var ok = await Shell.Current.DisplayAlert("Delete", $"Delete '{e.Description}'?", "Yes", "No");
        if (!ok) return;
        await RunSafeAsync(async () =>
        {
            await _service.DeleteAsync(e.Id);
            await LoadAsync();
            await Toast.Make("Expense deleted").Show();
        });
    }
}
