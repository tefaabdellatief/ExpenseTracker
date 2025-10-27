using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ExpenseTracker.App.Services;

public class DialogService : IDialogService
{
    public async Task ShowErrorAsync(string title, string message)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlert(title, message, "OK");
        }
        
        var toast = Toast.Make(message, ToastDuration.Short, 14);
        await toast.Show();
    }

    public async Task ShowSuccessAsync(string title, string message)
    {
        var toast = Toast.Make(message, ToastDuration.Short, 14);
        await toast.Show();
    }

    public async Task<bool> ShowConfirmAsync(string title, string message)
    {
        if (Shell.Current != null)
        {
            return await Shell.Current.DisplayAlert(title, message, "Yes", "No");
        }
        return false;
    }
}