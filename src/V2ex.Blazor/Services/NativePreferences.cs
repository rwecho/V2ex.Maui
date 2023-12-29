using System.Text.Json;

namespace V2ex.Blazor.Services;

public class NativePreferences: IPreferences
{
    public T Get<T>(string key, T defaultValue)
    {
        try
        {
            string? value = null;
            var result = Preferences.Get(key, value);
            if (result == null)
            {
                return defaultValue;
            }
            return JsonSerializer.Deserialize<T>(result) ?? defaultValue;
        }
        catch (JsonException)
        {
            return defaultValue;
        }
    }

    public void Set<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        Preferences.Set(key, json);
    }
}