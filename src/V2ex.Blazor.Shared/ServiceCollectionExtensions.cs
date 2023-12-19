using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using V2ex.Api;
using V2ex.Blazor.Services;

namespace V2ex.Blazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorShared(this IServiceCollection services)
    {
        // add http client and configure cookie handler
        // enable CORS for api
        services.AddHttpClient("api", client =>
        {
        })
            .ConfigurePrimaryHttpMessageHandler((sp) =>
             {
                 return new CookieHttpClientHandler(sp.GetRequiredService<IPreferences>());
             });
        
        // We register the AuthenticationStateProvider as a singleton
        // because we want to reuse the same instance for the entire app.
        services.AddSingleton<AuthenticationStateProvider, V2exAuthenticationStateProvider>();

        services.AddScoped<EmojiService>();
        services.AddScoped<ApiService>();
        services.AddScoped<UtilsJsInterop>();
        return services;
    }
}