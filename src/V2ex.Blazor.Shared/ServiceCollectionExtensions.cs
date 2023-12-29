using Microsoft.Extensions.DependencyInjection;
using V2ex.Api;
using V2ex.Blazor.Services;

namespace V2ex.Blazor;



public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorShared(this IServiceCollection services)
    {
        services.AddTransient<ChatGPTService>();
        services.AddScoped<EmojiService>();
        services.AddScoped<ApiService>();
        services.AddScoped<UtilsJsInterop>();
        return services;
    }
}