using Microsoft.AspNetCore.Components.WebView.Maui;

namespace V2ex.Blazor;

public class CustomBlazorWebViewHandler: BlazorWebViewHandler
{
    protected override Android.Webkit.WebView CreatePlatformView()
    {
        var webView = base.CreatePlatformView();

        webView.Settings.JavaScriptEnabled = true;
        webView.AddJavascriptInterface(new DocumentJsInterface(this), "interOp");

        return webView;
    }
}
