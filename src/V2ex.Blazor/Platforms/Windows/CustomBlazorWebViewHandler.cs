using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System.Text.Json;
using V2ex.Blazor.Pages;

namespace V2ex.Blazor;

public class CustomBlazorWebViewHandler : BlazorWebViewHandler
{
    protected override WebView2 CreatePlatformView()
    {
        return base.CreatePlatformView();
    }

    protected override void ConnectHandler(WebView2 platformView)
    {
        base.ConnectHandler(platformView);
        platformView.NavigationCompleted += PlatformView_NavigationCompleted;
    }

    private void PlatformView_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        sender.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
    }

    private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
    {

    }

    private record Message(string Type, string Payload);
}
