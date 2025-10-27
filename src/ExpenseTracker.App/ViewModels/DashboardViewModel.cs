using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTracker.Infrastructure.Interfaces;
using ExpenseTracker.App.Models;

namespace ExpenseTracker.App.ViewModels;
public partial class DashboardViewModel : BaseViewModel
{
    private readonly IExpenseService _service;
    
    [ObservableProperty] 
    private decimal totalExpenses;
    
    [ObservableProperty]
    private int totalTransactions;
    
    [ObservableProperty]
    private decimal averageExpense;

    public ObservableCollection<CategoryExpenseData> CategoryData { get; } = new();

    public DashboardViewModel(IExpenseService service) => _service = service;

    [RelayCommand]
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            var expenses = await _service.GetAllAsync();
            var expenseList = expenses.ToList();
            
            // Calculate totals
            TotalExpenses = expenseList.Sum(x => x.Amount);
            TotalTransactions = expenseList.Count;
            AverageExpense = TotalTransactions > 0 ? TotalExpenses / TotalTransactions : 0;
            
            // Group by category and calculate percentages
            CategoryData.Clear();
            var categoryGroups = expenseList
                .GroupBy(x => x.Category)
                .Select(g => new CategoryExpenseData
                {
                    Category = g.Key,
                    Amount = g.Sum(x => x.Amount),
                    Percentage = TotalExpenses > 0 ? (double)(g.Sum(x => x.Amount) / TotalExpenses * 100) : 0
                })
                .OrderByDescending(x => x.Amount)
                .ToList();
            
            foreach (var categoryData in categoryGroups)
            {
                CategoryData.Add(categoryData);
            }

            
            // If no data from service, add test data
            if (CategoryData.Count == 0)
            {
                CategoryData.Add(new CategoryExpenseData
                {
                    Category = Entities.Enums.Enums.ExpenseCategory.Food,
                    Amount = 150.50m,
                    Percentage = 45.5
                });
                CategoryData.Add(new CategoryExpenseData
                {
                    Category = Entities.Enums.Enums.ExpenseCategory.Transport,
                    Amount = 80.25m,
                    Percentage = 24.3
                });
            }
        });
    }
}