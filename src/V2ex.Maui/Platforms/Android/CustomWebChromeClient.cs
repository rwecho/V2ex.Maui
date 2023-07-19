using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace V2ex.Maui.Platforms.Android;

public class CustomWebChromeClient : MauiWebChromeClient
{
    public CustomWebChromeClient(IWebViewHandler handler) : base(handler)
    {
        handler.PlatformView.Settings.JavaScriptEnabled = true;
        handler.PlatformView.Settings.AllowContentAccess = true;
        handler.PlatformView.Settings.DomStorageEnabled = true;
    }
}