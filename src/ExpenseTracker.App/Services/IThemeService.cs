using Entities.Enums;

namespace ExpenseTracker.App.Services;

public interface IThemeService
{
    event Action ThemeChanged;

    AppThemes CurrentTheme { get; }
    void SetTheme(AppThemes theme);
    void ApplyTheme();
}