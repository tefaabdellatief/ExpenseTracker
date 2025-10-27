using ExpenseTracker.App.ViewModels;

namespace ExpenseTracker.App.Views;

public partial class AddEditExpensePage : ContentPage
{
    AddEditExpenseViewModel ViewModel;
    public AddEditExpensePage(AddEditExpenseViewModel vm)
    {
        InitializeComponent();
        BindingContext = ViewModel = vm;
    }
}
