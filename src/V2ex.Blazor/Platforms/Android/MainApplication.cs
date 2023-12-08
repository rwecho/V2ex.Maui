using Android.App;
using Android.Runtime;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace V2ex.Blazor;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{

    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
