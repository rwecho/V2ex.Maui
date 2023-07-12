namespace V2ex.Api;

public interface IPreferences
{
    void Set<T>(string key, T value);

    T Get<T>(string key, T defaultValue);
}
