using Foundation;
using UIKit;

namespace ExpenseTracker.App.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions);
        
        // Set status bar style
        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
        
        // Configure navigation bar appearance
        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
        {
            var appearance = new UINavigationBarAppearance();
            appearance.ConfigureWithOpaqueBackground();
            appearance.BackgroundColor = UIColor.FromRGB(255, 255, 255); // #ffffff
            appearance.TitleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(10, 76, 71) // #0a4c47
            };
            
            UINavigationBar.Appearance.StandardAppearance = appearance;
            UINavigationBar.Appearance.ScrollEdgeAppearance = appearance;
        }
        else
        {
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(10, 76, 71);
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(10, 76, 71)
            };
        }

        // Configure tab bar appearance
        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
        {
            var tabAppearance = new UITabBarAppearance();
            tabAppearance.ConfigureWithOpaqueBackground();
            tabAppearance.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            
            UITabBar.Appearance.StandardAppearance = tabAppearance;
            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                UITabBar.Appearance.ScrollEdgeAppearance = tabAppearance;
            }
        }
        else
        {
            UITabBar.Appearance.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            UITabBar.Appearance.TintColor = UIColor.FromRGB(10, 76, 71);
        }
        
        return result;
    }
}