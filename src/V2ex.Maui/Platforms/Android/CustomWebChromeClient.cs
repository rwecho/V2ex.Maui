using Android.OS;
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

        handler.PlatformView.AddJavascriptInterface(new InteropInterface(handler.VirtualView), "interop");
        this.WebView = handler.VirtualView;
    }

    private IWebView WebView { get; }

    public override void OnProgressChanged(global::Android.Webkit.WebView? view, int newProgress)
    {
        if (newProgress == 100)
        {
            var script = @"var height = document.body.scrollHeight;
                          interop.setHeight(height);";
            this.WebView.Eval(script);
        }
    }
}