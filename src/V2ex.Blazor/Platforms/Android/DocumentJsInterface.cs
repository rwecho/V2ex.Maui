using Android.Webkit;
using Java.Interop;
using V2ex.Blazor.Pages;

namespace V2ex.Blazor;

public class DocumentJsInterface : Java.Lang.Object
{
    private readonly CustomBlazorWebViewHandler _handler;

    public DocumentJsInterface(CustomBlazorWebViewHandler handler)
    {
        _handler = handler;
    }
}