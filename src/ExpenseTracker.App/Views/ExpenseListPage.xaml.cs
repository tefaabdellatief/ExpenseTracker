using ExpenseTracker.App.ViewModels;

namespace ExpenseTracker.App.Views;

public partial class ExpenseListPage : ContentPage
{
    ExpenseListViewModel ViewModel;
    public ExpenseListPage(ExpenseListViewModel vm)
    {
        InitializeComponent();
        BindingContext = ViewModel = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ViewModel.LoadAsync();
    }
}
