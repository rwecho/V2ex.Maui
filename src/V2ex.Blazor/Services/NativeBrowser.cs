namespace V2ex.Blazor.Services;

public class NativeBrowser : IBrowser
{
    public async Task OpenAsync(Uri uri)
    {
        await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
}
