using CommunityToolkit.Maui.Alerts;
using V2ex.Blazor.Services;

namespace V2ex.Blazor.Services;

public class NativeAlterService : IAlterService
{
    private Page MainPage
    {
        get
        {
            return Application.Current!.MainPage!;
        }
    }

    public async Task<bool> Confirm(string title, string message, string accept = "确定", string cancel = "取消")
    {
        return await MainPage.DisplayAlert(title, message, accept, cancel);
    }
}
