using Android.Webkit;
using Java.Interop;

namespace V2ex.Maui.Platforms.Android;

public class InteropInterface : Java.Lang.Object
{
    public InteropInterface(IWebView virtualView)
    {
        this.WebView = (Microsoft.Maui.Controls.WebView)virtualView;
    }
    private Microsoft.Maui.Controls.WebView WebView { get; }

    [JavascriptInterface]
    [Export("setHeight")]
    public void SetHeight(int height)
    {
        if(WebView.HeightRequest == height)
        {
            return;
        }
        WebView.Dispatcher.Dispatch(() =>
        {
            WebView.HeightRequest = height;
        });
    }
}