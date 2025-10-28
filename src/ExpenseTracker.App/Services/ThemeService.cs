using Entities.Enums;
using System.Diagnostics.CodeAnalysis;
using MauiAppTheme = Microsoft.Maui.ApplicationModel.AppTheme;

namespace ExpenseTracker.App.Services;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
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
                try
                {
                    var savedTheme = Preferences.Get(ThemeKey, nameof(AppThemes.System));
                    _currentTheme = Enum.Parse<AppThemes>(savedTheme);
                }
                catch (Exception)
                {
                    _currentTheme = AppThemes.System;
                    Preferences.Set(ThemeKey, AppThemes.System.ToString());
                }
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
        try
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
            
            if (Application.Current?.Resources == null) 
            {
                System.Diagnostics.Debug.WriteLine("Application.Current or Resources is null, cannot apply theme");
                return;
            }
            
            // Determine which theme to apply
            var themeToApply = CurrentTheme;
            if (themeToApply == AppThemes.System)
            {
                themeToApply = Application.Current.RequestedTheme == MauiAppTheme.Dark ? AppThemes.Dark : AppThemes.Light;
            }
            
            // Load and merge the appropriate theme resource dictionary
            LoadThemeResources(themeToApply);
            
            // Set the MAUI UserAppTheme for system integration
            switch (themeToApply)
            {
                case AppThemes.Light:
                    Application.Current.UserAppTheme = MauiAppTheme.Light;
                    break;
                case AppThemes.Dark:
                    Application.Current.UserAppTheme = MauiAppTheme.Dark;
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error applying theme: {ex.Message}");
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
                resources.MergedDictionaries.Add(themeResources);
                System.Diagnostics.Debug.WriteLine($"Successfully loaded {theme} theme resources");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load {theme} theme resources - using fallback");
                // Fallback: ensure we have at least light theme loaded
                if (!resources.MergedDictionaries.Any(rd => rd.Source?.OriginalString?.Contains("LightTheme.xaml") == true))
                {
                    var fallback = LoadResourceDictionary("Themes/LightTheme.xaml");
                    if (fallback != null)
                    {
                        resources.MergedDictionaries.Add(fallback);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading theme resources: {ex.Message}");
        }
    }

    private ResourceDictionary? LoadResourceDictionary(string resourcePath)
    {
        try
        {
            var uriFormats = new[]
            {
                $"ms-appx:///{resourcePath}",
                $"/{resourcePath}",
                resourcePath
            };

            foreach (var uriFormat in uriFormats)
            {
                try
                {
                    var uri = new Uri(uriFormat, UriKind.RelativeOrAbsolute);
                    var resourceDict = new ResourceDictionary { Source = uri };
                    return resourceDict;
                }
                catch (Exception innerEx)
                {
                    continue;
                }
            }
            
            return null;
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
        //if (CurrentTheme == AppThemes.System)
        //{
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ApplyTheme();
                ThemeChanged?.Invoke();
            });
        //}
    }
}