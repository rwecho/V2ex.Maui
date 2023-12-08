using Android.App;
using Android.Content.PM;

namespace V2ex.Blazor;

[Activity(Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode 
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    WindowSoftInputMode = Android.Views.SoftInput.AdjustResize | Android.Views.SoftInput.StateHidden
    )]
public class MainActivity : MauiAppCompatActivity
{
}
