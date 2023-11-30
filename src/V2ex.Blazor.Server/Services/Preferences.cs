using Microsoft.AspNetCore.Components.Routing;
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

public class NavigationInterceptorService : INavigationInterceptorService
{
    public Task Intercept(string sourceLocation, LocationChangingContext context)
    {
        return Task.CompletedTask;
    }
}