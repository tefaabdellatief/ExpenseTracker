namespace ExpenseTracker.App.Services;

public interface IDialogService
{
    Task ShowErrorAsync(string title, string message);
    Task ShowSuccessAsync(string title, string message);
    Task<bool> ShowConfirmAsync(string title, string message);
}