using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using ExpenseTracker.Infrastructure.Interfaces;
using static Entities.Enums.Enums;
using Entities.Dtos;

namespace ExpenseTracker.App.ViewModels;

public partial class AddEditExpenseViewModel : BaseViewModel
{
    private readonly IExpenseService _service;

    public Array Categories => Enum.GetValues(typeof(ExpenseCategory));

    [ObservableProperty] private decimal amount;
    [ObservableProperty] private ExpenseCategory category;
    [ObservableProperty] private DateTime date = DateTime.Today;
    [ObservableProperty] private string description = string.Empty;

    public AddEditExpenseViewModel(IExpenseService service) => _service = service;

    [RelayCommand]
    public async Task SaveAsync()
    {
        // validations
        if (Amount <= 0)
        {
            await Shell.Current.DisplayAlert("Validation", "Amount must be greater than zero.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlert("Validation", "Description is required.", "OK");
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Confirm", "Save this expense?", "Yes", "No");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            var e = new ExpenseDto { Amount = Amount, Category = Category, Date = Date, Description = Description, CreatedAt = DateTime.UtcNow };
            await _service.AddAsync(e);
            await Toast.Make("Expense saved").Show();
            await Shell.Current.GoToAsync("..");
        });
    }
}