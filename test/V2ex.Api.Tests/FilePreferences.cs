using System.Text.Json;
using Volo.Abp.DependencyInjection;

namespace V2ex.Api.Tests;

public class FilePreferences :  ITransientDependency
{
    public T Get<T>(string key, T defaultValue)
    {
        if (!File.Exists(key))
        {
            return defaultValue;
        }
        var json = File.ReadAllText(key);
        try
        {
            return JsonSerializer.Deserialize<T>(json) ?? defaultValue;
        }
        catch (JsonException)
        {
            return defaultValue;
        }
    }

    public void Set<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(key, json);
    }
}

