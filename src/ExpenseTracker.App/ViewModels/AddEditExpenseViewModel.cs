using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using ExpenseTracker.Infrastructure.Interfaces;
using static Entities.Enums.Enums;
using Entities.Dtos;

namespace ExpenseTracker.App.ViewModels;

public partial class AddEditExpenseViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IExpenseService _service;
    private int _expenseId;

    public Array Categories => Enum.GetValues(typeof(ExpenseCategory));

    [ObservableProperty] private decimal amount;
    [ObservableProperty] private ExpenseCategory category;
    [ObservableProperty] private DateTime date = DateTime.Today;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string pageTitle = "Add Expense";
    [ObservableProperty] private bool isEditMode = false;
    [ObservableProperty] private bool isViewMode = false;
    [ObservableProperty] private bool isEditing = false;
    [ObservableProperty] private string editButtonText = "Edit";

    public AddEditExpenseViewModel(IExpenseService service) => _service = service;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ExpenseId") && int.TryParse(query["ExpenseId"].ToString(), out int expenseId))
        {
            _expenseId = expenseId;
            IsEditMode = true;
            IsViewMode = true;
            IsEditing = false;
            PageTitle = "Expense Details";
            LoadExpenseAsync();
        }
        else
        {
            IsEditMode = false;
            IsViewMode = false;
            IsEditing = true;
            PageTitle = "Add Expense";
            ResetForm();
        }
    }

    private async void LoadExpenseAsync()
    {
        await RunSafeAsync(async () =>
        {
            var expense = await _service.GetByIdAsync(_expenseId);
            if (expense != null)
            {
                Amount = expense.Amount;
                Category = expense.Category;
                Date = expense.Date;
                Description = expense.Description;
            }
        });
    }

    private void ResetForm()
    {
        Amount = 0;
        Category = ExpenseCategory.Food;
        Date = DateTime.Today;
        Description = string.Empty;
        _expenseId = 0;
    }

    [RelayCommand]
    public void ToggleEdit()
    {
        IsEditing = !IsEditing;
        PageTitle = IsEditing ? "Edit Expense" : "Expense Details";
        EditButtonText = IsEditing ? "Cancel" : "Edit";
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (!IsEditMode) return;
        
        if (Shell.Current == null) return;
        
        var confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete this expense?", "Yes", "No");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            await _service.DeleteAsync(_expenseId);
            await Toast.Make("Expense deleted").Show();
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("..");
            }
        });
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        // validations
        if (Amount <= 0)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Validation", "Amount must be greater than zero.", "OK");
            }
            return;
        }
        if (string.IsNullOrWhiteSpace(Description))
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Validation", "Description is required.", "OK");
            }
            return;
        }

        if (Shell.Current == null) return;
        var confirm = await Shell.Current.DisplayAlert("Confirm", "Save this expense?", "Yes", "No");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            if (IsEditMode)
            {
                var e = new ExpenseDto 
                { 
                    Id = _expenseId,
                    Amount = Amount, 
                    Category = Category, 
                    Date = Date, 
                    Description = Description,
                    ModifiedAt = DateTime.UtcNow 
                };
                await _service.UpdateAsync(e);
                await Toast.Make("Expense updated").Show();
                IsEditing = false;
                PageTitle = "Expense Details";
                EditButtonText = "Edit";
            }
            else
            {
                var e = new ExpenseDto 
                { 
                    Amount = Amount, 
                    Category = Category, 
                    Date = Date, 
                    Description = Description, 
                    CreatedAt = DateTime.UtcNow 
                };
                await _service.AddAsync(e);
                await Toast.Make("Expense saved").Show();
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
        });
    }
}