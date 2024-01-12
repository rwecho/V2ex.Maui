using Android.App;
using Android.Content.PM;
using Android.OS;

namespace V2ex.Blazor;

[Activity(Theme = "@style/SplashTheme", 
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode 
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    WindowSoftInputMode = Android.Views.SoftInput.AdjustResize | Android.Views.SoftInput.StateHidden
    )]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if(Window!=null)
        {
            // we can use SetNavigationBarColor to set the color
        }
    }
}
