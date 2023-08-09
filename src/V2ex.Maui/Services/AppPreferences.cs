

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
