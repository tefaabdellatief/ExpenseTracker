namespace ExpenseTracker.App.Services;

public interface IPreferencesService
{
    bool IsLoggedIn { get; set; }
    string UserEmail { get; set; }
    void ClearUserData();
}