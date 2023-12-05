using Android.Webkit;
using Java.Interop;
using Microsoft.AspNetCore.Components.WebView.Maui;
using V2ex.Blazor.Pages;

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

public class DocumentJsInterface: Java.Lang.Object
{
    private readonly CustomBlazorWebViewHandler _handler;

    public DocumentJsInterface(CustomBlazorWebViewHandler handler)
    {
        _handler = handler;
    }

    [JavascriptInterface]
    [Export("setTitle")]
    public void SetTitle(string title)
    {
        var view = (BindableObject)_handler.VirtualView;
        WebViewEvents.GetTitleChangeCommand(view)?.Execute(title);
    }
}