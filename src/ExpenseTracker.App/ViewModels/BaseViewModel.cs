using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.App.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;

    protected async Task RunSafeAsync(Func<Task> action)
    {
        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}