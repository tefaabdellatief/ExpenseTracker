using ExpenseTracker.App.ViewModels;
using System.Collections.Specialized;

namespace ExpenseTracker.App.Views;

public partial class DashboardPage : ContentPage
{
    DashboardViewModel ViewModel;
    
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;

        ViewModel.CategoryData.CollectionChanged += OnCategoryDataChanged;
        UpdateEmptyStateVisibility();
    }

    private void OnCategoryDataChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateEmptyStateVisibility();
    }

    private void UpdateEmptyStateVisibility()
    {
        EmptyStateContainer.IsVisible = ViewModel.CategoryData.Count == 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadAsync();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ViewModel.LoadAsync();
        UpdateEmptyStateVisibility();
    }
}