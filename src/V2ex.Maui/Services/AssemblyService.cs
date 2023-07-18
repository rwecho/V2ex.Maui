using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class AssemblyService : ITransientDependency
{
    public string GetVersion()
    {
        var assembly = typeof(AssemblyService).Assembly;
        var version = assembly.GetName().Version;
        if (version == null)
        {
            return "1.0.0";
        }

        return $"{version.Major}.{version.Minor}.{version.Build}";
    }
}