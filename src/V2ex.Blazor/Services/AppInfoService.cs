namespace V2ex.Blazor.Services;

public class AppInfoService : IAppInfoService
{
    public string GetVersion()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        var version = AppInfo.Current.Version;
#pragma warning restore CA1416 // Validate platform compatibility
        return $"{version.Major}.{version.Minor}.{version.Revision}";
    }

    public int GetVersionNumber()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        var version = AppInfo.Current.Version;
#pragma warning restore CA1416 // Validate platform compatibility
        return int.Parse($"{version.Major:D3}{version.Minor:D3}{version.Revision:D3}");

    }
}
