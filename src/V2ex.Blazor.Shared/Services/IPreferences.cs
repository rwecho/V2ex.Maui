namespace V2ex.Blazor.Services;

public interface IPreferences
{
    T Get<T>(string key, T defaultValue);

    void Set<T>(string key, T value);
}