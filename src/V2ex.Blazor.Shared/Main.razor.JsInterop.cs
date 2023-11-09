
using Microsoft.JSInterop;

public class MainJsInterop : IAsyncDisposable
{

    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public MainJsInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/V2ex.Blazor.Shared/main.js").AsTask());
    }

    public async ValueTask Initialize()
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("initialize");
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}