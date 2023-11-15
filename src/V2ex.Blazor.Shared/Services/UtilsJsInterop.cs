using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
namespace V2ex.Blazor.Services;

public class UtilsJsInterop 
{
    private IJSRuntime JsRuntime { get; }

    public UtilsJsInterop(IJSRuntime jsRuntime)
    {
       this.JsRuntime = jsRuntime;
    }

    public ValueTask ScrollToElement(ElementReference eleRef)
    {
        return this.JsRuntime.InvokeVoidAsync("utils.scrollToElement", eleRef, "hello");
    }
}
