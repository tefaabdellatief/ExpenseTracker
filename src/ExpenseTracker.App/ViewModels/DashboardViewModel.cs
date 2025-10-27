using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Infrastructure.Interfaces;
using static Entities.Enums.Enums;

namespace ExpenseTracker.App.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IExpenseService _service;
    [ObservableProperty] private decimal total;

    public ObservableCollection<(ExpenseCategory Category, decimal Total)> CategoryTotals { get; } = new();

    public DashboardViewModel(IExpenseService service) => _service = service;

    [RelayCommand]
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            var list = await _service.GetAllAsync();
            Total = list.Sum(x => x.Amount);
            CategoryTotals.Clear();
            foreach (var g in list.GroupBy(x => x.Category))
                CategoryTotals.Add((g.Key, g.Sum(x => x.Amount)));
        });
    }
}