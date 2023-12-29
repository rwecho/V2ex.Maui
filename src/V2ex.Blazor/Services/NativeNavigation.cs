using V2ex.Blazor.Pages;

namespace V2ex.Blazor.Services;

public class NativeNavigation : INativeNavigation
{
    private INavigation Navigation
    {
        get
        {
            return Application.Current!.MainPage!.Navigation;
        }
    }

    public Task GoBack()
    {
        return Navigation.PopAsync(true);
    }

    public async Task<bool> GoLoginWithGooglePage(string once)
    {
        var tsc = new TaskCompletionSource<bool>();

        Action<bool> LoginCallback = tsc.SetResult;

        await Navigation.PushAsync(new LoginWithGooglePage(once, LoginCallback), false);

        return await tsc.Task;
    }
}