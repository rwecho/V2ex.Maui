using CommunityToolkit.Maui;
#if ! IOS
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using V2ex.Blazor.Pages;
using V2ex.Blazor.Services;
using IPreferences = V2ex.Blazor.Services.IPreferences;

namespace V2ex.Blazor;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.ConfigureMauiHandlers(builder => builder.AddHandler<BlazorWebView, CustomBlazorWebViewHandler>());

        // add http client and configure cookie handler
        // enable CORS for api
        builder.Services.AddHttpClient("api", client =>
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgentConstants.UserAgent);
        })
            .ConfigurePrimaryHttpMessageHandler((sp) =>
            {
                return sp.GetRequiredService<ApiHttpClientHandler>();
            });

        builder.Services.AddHttpClient("ai", client =>
        {

        })
            .ConfigurePrimaryHttpMessageHandler((sp) => {
                return sp.GetRequiredService<AiHttpClientHandler>();
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorShared();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddTransient<Services.IBrowser, Services.NativeBrowser>();
        builder.Services.AddTransient<ApiHttpClientHandler>();
        builder.Services.AddTransient<AiHttpClientHandler>();
        builder.Services.AddTransient<LoginWithGooglePageViewModel>();
        builder.Services.AddScoped<INavigationInterceptorService, NavigationInterceptorService>();
        builder.Services.AddScoped<INativeNavigation, NativeNavigation>();
        builder.Services.AddScoped<IAlterService, NativeAlterService>();
        builder.Services.AddScoped<IPreferences, NativePreferences>();
        builder.Services.AddScoped<IToastService, NativeToastService>();
        builder.Services.AddScoped<IAppInfoService, AppInfoService>();
        builder.Services.AddSingleton<CookieContainerService>();
        builder.Services.AddSingleton<AuthenticationStateProvider, V2exAuthenticationStateProvider>();
        builder.Services.AddSingleton<IAuthenticationStateProvider, V2exAuthenticationStateProvider>();

        ConfigureConfiguration(builder);

        var configuration = builder.Configuration;
        builder.Services.Configure<AppCenterOptions>(configuration.GetSection("AppCenter"));

        builder.Services.Configure<ChatGPTOptions>(configuration.GetSection("ChatGPT"));


#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        var app = builder.Build();
        var appCenterOptions = app.Services.GetRequiredService<IOptions<AppCenterOptions>>();

        InstanceCreator.Initialize(app.Services);

#if ANDROID
        AppCenter.Start(appCenterOptions.Value.AndroidSecret, typeof(Analytics), typeof(Crashes));
#endif
        return app;
    }

    private static void ConfigureConfiguration(MauiAppBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
    }
}
