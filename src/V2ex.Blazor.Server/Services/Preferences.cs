using V2ex.Api;

namespace V2ex.Blazor.Services;

public class Preferences : IPreferences
{
    public T Get<T>(string key, T defaultValue)
    {
        return defaultValue;    
    }

    public void Set<T>(string key, T value)
    {
    }
}
