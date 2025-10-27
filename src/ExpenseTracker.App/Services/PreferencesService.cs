namespace ExpenseTracker.App.Services;

public class PreferencesService : IPreferencesService
{
    private const string IsLoggedInKey = "IsLoggedIn";
    private const string UserEmailKey = "UserEmail";

    public bool IsLoggedIn
    {
        get => Preferences.Get(IsLoggedInKey, false);
        set => Preferences.Set(IsLoggedInKey, value);
    }

    public string UserEmail
    {
        get => Preferences.Get(UserEmailKey, string.Empty);
        set => Preferences.Set(UserEmailKey, value);
    }

    public void ClearUserData()
    {
        Preferences.Remove(IsLoggedInKey);
        Preferences.Remove(UserEmailKey);
    }
}