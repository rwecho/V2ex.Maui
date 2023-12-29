namespace V2ex.Blazor.Services;

public interface INativeNavigation
{
    Task GoBack();

    Task<bool> GoLoginWithGooglePage(string once);
}
