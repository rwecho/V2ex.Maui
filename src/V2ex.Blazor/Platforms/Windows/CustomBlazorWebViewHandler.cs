using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System.Text.Json;
using V2ex.Blazor.Pages;

namespace V2ex.Blazor;

public class CustomBlazorWebViewHandler: BlazorWebViewHandler
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
        if (!args.WebMessageAsJson.Contains("setTitle"))
        {
            return;
        }

        Message message;
        try
        {
            message = JsonSerializer.Deserialize<Message>(args.TryGetWebMessageAsString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidCastException();
        }
        catch (Exception)
        {
            return;
        }

        var view = (BindableObject)this.VirtualView;

        if (message.Type == "setTitle")
        {
            WebViewEvents.GetTitleChangeCommand(view)?.Execute(message.Payload);
        }
    }

    private record Message (string Type, string Payload);
}
