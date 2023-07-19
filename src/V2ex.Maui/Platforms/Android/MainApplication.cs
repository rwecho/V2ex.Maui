using Android.App;
using Android.Runtime;
using V2ex.Maui.Platforms.Android;

namespace V2ex.Maui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                Microsoft.Maui.Handlers.WebViewHandler.Mapper.ModifyMapping(
                    nameof(Android.Webkit.WebView.WebChromeClient),
                    (handler, view, args) => handler.PlatformView.SetWebChromeClient(new CustomWebChromeClient(handler)));
            }

            var app = MauiProgram.CreateMauiApp(FilesDir!.AbsolutePath);

            return app;
        }
    }
}