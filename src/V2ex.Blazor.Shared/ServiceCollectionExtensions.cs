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

        services.AddHttpClient("ai", client =>
        {

        })
            .ConfigurePrimaryHttpMessageHandler((sp)=> {

#pragma warning disable CA1416 // Validate platform compatibility
                return sp.GetRequiredService<AiHttpClientHandler>();
#pragma warning restore CA1416 // Validate platform compatibility
            });

        // We register the AuthenticationStateProvider as a singleton
        // because we want to reuse the same instance for the entire app.
        services.AddSingleton<AuthenticationStateProvider, V2exAuthenticationStateProvider>();
        services.AddTransient<ChatGPTService>();
        services.AddTransient<AiHttpClientHandler>();

        services.AddScoped<EmojiService>();
        services.AddScoped<ApiService>();
        services.AddScoped<UtilsJsInterop>();
        return services;
    }
}