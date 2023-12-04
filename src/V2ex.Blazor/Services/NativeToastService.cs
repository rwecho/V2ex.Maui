using CommunityToolkit.Maui.Alerts;

namespace V2ex.Blazor.Services;

public class NativeToastService : Services.IToastService
{
    public Task Show(string text)
    {
        return Toast.Make(text).Show();
    }
}