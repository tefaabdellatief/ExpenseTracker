using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entities.Dtos;
using Entities.Enums;
using ExpenseTracker.Infrastructure.Interfaces;
using System.Collections.ObjectModel;

namespace ExpenseTracker.App.ViewModels;

public partial class ExpenseListViewModel : BaseViewModel
{
    private readonly IExpenseService _service;
    private List<ExpenseDto> _allExpenses = new();

    public ObservableCollection<ExpenseDto> Expenses { get; } = new();
    public ObservableCollection<string> Categories { get; } = new();
    
    [ObservableProperty] 
    private string searchText = string.Empty;
    
    [ObservableProperty]
    private string? selectedCategory;
    
    [ObservableProperty]
    private DateTime? fromDate;
    
    [ObservableProperty]
    private DateTime? toDate;
    
    [ObservableProperty]
    private bool isFilterExpanded = false;
    
    [ObservableProperty]
    private int activeFilterCount = 0;
    
    [ObservableProperty]
    private bool isRefreshing = false;
    
    public bool HasActiveFilters => ActiveFilterCount > 0;

    public ExpenseListViewModel(IExpenseService service) 
    {
        _service = service;
        InitializeCategories();
    }

    private void InitializeCategories()
    {
        Categories.Add("All Categories");
        foreach (var category in Enum.GetValues<Enums.ExpenseCategory>())
        {
            Categories.Add(category.ToString());
        }
        SelectedCategory = "All Categories";
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFiltersAsync();
    }
    
    partial void OnSelectedCategoryChanged(string? value)
    {
        ApplyFiltersAsync();
    }
    
    partial void OnFromDateChanged(DateTime? value)
    {
        ApplyFiltersAsync();
    }
    
    partial void OnToDateChanged(DateTime? value)
    {
        ApplyFiltersAsync();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            var items = await _service.GetAllAsync();
            _allExpenses = items.ToList();
            ApplyFiltersAsync();
        });
    }

    [RelayCommand]
    public void ToggleFilter()
    {
        IsFilterExpanded = !IsFilterExpanded;
    }
    
    [RelayCommand]
    public void ClearFilters()
    {
        SelectedCategory = "All Categories";
        FromDate = null;
        ToDate = null;
        SearchText = string.Empty;
        UpdateFilterCount();
    }
    
    private void UpdateFilterCount()
    {
        int count = 0;
        if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All Categories") count++;
        if (FromDate.HasValue) count++;
        if (ToDate.HasValue) count++;
        if (!string.IsNullOrWhiteSpace(SearchText)) count++;
        
        ActiveFilterCount = count;
        OnPropertyChanged(nameof(HasActiveFilters));
    }

    private async void ApplyFiltersAsync()
    {
        await RunSafeAsync(async () =>
        {
            IEnumerable<ExpenseDto> filteredExpenses;

            // Use service methods for category and date filtering
            bool hasCategory = !string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All Categories";
            bool hasDateRange = FromDate.HasValue && ToDate.HasValue;

            if (hasCategory && hasDateRange)
            {
                filteredExpenses = await _service.FilterByCategoryAndDateRangeAsync(
                    SelectedCategory!, FromDate!.Value, ToDate!.Value);
            }
            else if (hasCategory)
            {
                filteredExpenses = await _service.FilterByCategoryAsync(SelectedCategory!);
            }
            else if (hasDateRange)
            {
                filteredExpenses = await _service.FilterByDateRangeAsync(FromDate!.Value, ToDate!.Value);
            }
            else
            {
                filteredExpenses = _allExpenses;
            }

            // Apply text search on the filtered results
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchTerm = SearchText.ToLowerInvariant();
                filteredExpenses = filteredExpenses.Where(expense =>
                    expense.Description.ToLowerInvariant().Contains(searchTerm) ||
                    expense.Amount.ToString().Contains(searchTerm) ||
                    expense.Category.ToString().ToLowerInvariant().Contains(searchTerm));
            }

            Expenses.Clear();
            foreach (var expense in filteredExpenses.OrderByDescending(e => e.Date))
            {
                Expenses.Add(expense);
            }
            
            UpdateFilterCount();
        });
    }

    [RelayCommand]
    public async Task NavigateAddAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(nameof(Views.AddEditExpensePage));
        }
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task EditAsync(ExpenseDto expense)
    {
        if (expense == null || Shell.Current == null) return;
        await Shell.Current.GoToAsync($"{nameof(Views.AddEditExpensePage)}?ExpenseId={expense.Id}");
    }

    [RelayCommand]
    public async Task ViewDetailsAsync(ExpenseDto expense)
    {
        if (expense == null || Shell.Current == null) return;
        await Shell.Current.GoToAsync($"{nameof(Views.AddEditExpensePage)}?ExpenseId={expense.Id}");
    }

    [RelayCommand]
    public async Task DeleteAsync(ExpenseDto e)
    {
        if (e == null || Shell.Current == null) return;
        var ok = await Shell.Current.DisplayAlert("Delete", $"Delete '{e.Description}'?", "Yes", "No");
        if (!ok) return;
        await RunSafeAsync(async () =>
        {
            await _service.DeleteAsync(e.Id);
            _allExpenses.RemoveAll(x => x.Id == e.Id);
            ApplyFiltersAsync();
            await Toast.Make("Expense deleted").Show();
        });
    }
}