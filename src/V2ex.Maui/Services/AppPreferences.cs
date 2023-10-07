

using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public enum NightMode
{
    Close,
    Open,
    FollowBySystem
}

public class AppPreferences
{
    public string? LatestTabName { get; set; }

    public NightMode NightMode { get; set; }
}


public record AppState(string? HtmlContainer);
public static class AppStateManager
{
    public static AppState AppState { get; private set; } = new AppState(null);

    public static void SetHtmlContainer(string html)
    {
        AppState = new AppState(html);
    }
}
