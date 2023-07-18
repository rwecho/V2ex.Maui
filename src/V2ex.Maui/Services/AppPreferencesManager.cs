using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class AppPreferencesManager : PreferencesManager<AppPreferences>, ISingletonDependency
{
    public AppPreferencesManager() : base(nameof(AppPreferences))
    {
    }
}
