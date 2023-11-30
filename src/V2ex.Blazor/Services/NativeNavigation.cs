namespace V2ex.Blazor.Services;

public class NativeNavigation : INativeNavigation
{
    private INavigation Navigation
    {
        get
        {
            return App.Current!.MainPage!.Navigation;
        }
    }

    public Task GoBack()
    {
        return Navigation.PopAsync(true);
    }
}