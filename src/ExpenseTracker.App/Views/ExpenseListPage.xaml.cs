using ExpenseTracker.App.Services;
using ExpenseTracker.App.ViewModels;

namespace ExpenseTracker.App.Views;

public partial class ExpenseListPage : ContentPage
{
    ExpenseListViewModel ViewModel;
    private readonly IThemeService _themeService;
    public ExpenseListPage(ExpenseListViewModel vm, IThemeService themeService)
    {
        InitializeComponent();
        BindingContext = ViewModel = vm;
        _themeService = themeService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ViewModel.LoadAsync();
        //_themeService.ApplyTheme();
    }
}
