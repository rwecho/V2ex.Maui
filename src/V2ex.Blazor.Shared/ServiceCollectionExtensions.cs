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
             })
            // .AddHttpMessageHandler((sp) =>
            // {
            //     return new LoggingHttpMessageHandler(sp.GetRequiredService<ILogger<ApiService>>());
            // })
            ;

        services.AddScoped<AuthenticationStateProvider, V2exAuthenticationStateProvider>();
        services.AddScoped<ApiService>();
        services.AddScoped<UtilsJsInterop>();
        services.AddScoped<CurrentUser>();
        return services;
    }
}