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

        var major = version.Major < 0 ? 0 : version.Major;
        var minor = version.Minor < 0 ? 0 : version.Minor;
        var revision = version.Revision < 0 ? 0 : version.Revision;
        return int.Parse($"{major:D3}{minor:D3}{revision:D3}");
    }
}
