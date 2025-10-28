using Entities.Enums;
using MauiAppTheme = Microsoft.Maui.ApplicationModel.AppTheme;

namespace ExpenseTracker.App.Services;

public class ThemeService : IThemeService
{
    private const string ThemeKey = "AppTheme";
    private AppThemes _currentTheme;
    private bool _isInitialized = false;

    public event Action? ThemeChanged;

    public AppThemes CurrentTheme
    {
        get
        {
            if (_currentTheme == default)
            {
                var savedTheme = Preferences.Get(ThemeKey, nameof(AppTheme.Light));
                _currentTheme = Enum.Parse<AppThemes>(savedTheme);
            }
            return _currentTheme;
        }
    }

    public void SetTheme(AppThemes theme)
    {
        _currentTheme = theme;
        Preferences.Set(ThemeKey, theme.ToString());
        // UI updates
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ApplyTheme();
            ThemeChanged?.Invoke();
        });
    }

    public void ApplyTheme()
    {
        // Initialize system theme listener on first call
        if (!_isInitialized)
        {
            _isInitialized = true;
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeChanged += OnSystemThemeChanged;
            }
        }
        if (Application.Current?.Resources == null) return;
        // Determine which theme to apply
        var themeToApply = CurrentTheme;
        if (themeToApply == AppThemes.System)
        {
            themeToApply = Application.Current.RequestedTheme == MauiAppTheme.Dark ? AppThemes.Dark : AppThemes.Light;
        }
        // Load and merge the appropriate theme resource dictionary
        LoadThemeResources(themeToApply);
        switch (themeToApply)
        {
            case AppThemes.Light:
                System.Diagnostics.Debug.WriteLine("Applying Light Theme");
                Application.Current.UserAppTheme = MauiAppTheme.Light;
                break;
            case AppThemes.Dark:
                System.Diagnostics.Debug.WriteLine("Applying Dark Theme");
                Application.Current.UserAppTheme = MauiAppTheme.Dark;
                break;
        }
    }

    private void LoadThemeResources(AppThemes theme)
    {
        try
        {
            var resources = Application.Current?.Resources;
            if (resources == null) return;

            // Remove any existing theme
            RemoveExistingThemeResources(resources);

            // Load the appropriate theme resource dictionary
            ResourceDictionary? themeResources = theme switch
            {
                AppThemes.Dark => LoadResourceDictionary("Themes/DarkTheme.xaml"),
                _ => LoadResourceDictionary("Themes/LightTheme.xaml")
            };

            if (themeResources != null)
            {
                // Add the theme resources to the application resources
                resources.MergedDictionaries.Add(themeResources);
                System.Diagnostics.Debug.WriteLine($"Successfully loaded {theme} theme resources");
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    private ResourceDictionary? LoadResourceDictionary(string resourcePath)
    {
        try
        {
            var uri = new Uri($"ms-appx:///{resourcePath}");
            var resourceDict = new ResourceDictionary { Source = uri };
            return resourceDict;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void RemoveExistingThemeResources(ResourceDictionary resources)
    {
        // Remove any existing theme resource dictionaries
        var toRemove = resources.MergedDictionaries
            .Where(rd => rd.Source?.OriginalString?.Contains("Theme.xaml") == true)
            .ToList();

        foreach (var rd in toRemove)
        {
            resources.MergedDictionaries.Remove(rd);
        }
    }

    private void OnSystemThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        // Only respond to system theme changes if current theme is set to System
        if (CurrentTheme == AppThemes.System)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ApplyTheme();
                ThemeChanged?.Invoke();
            });
        }
    }
}