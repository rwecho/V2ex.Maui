using CommunityToolkit.Maui;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;
using V2ex.Blazor.Services;

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

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorShared();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddTransient<Services.IBrowser, Services.NativeBrowser>();
        builder.Services.AddTransient<Api.IPreferences, MauiPreferences>();
        builder.Services.AddScoped<INavigationInterceptorService, NavigationInterceptorService>();
        builder.Services.AddScoped<INativeNavigation, NativeNavigation>();
        builder.Services.AddScoped<IAlterService, NativeAlterService>();
        builder.Services.AddScoped<IToastService, NativeToastService>();

        ConfigureConfiguration(builder);

        var configuration = builder.Configuration;
        builder.Services.Configure<AppCenterOptions>(configuration.GetSection("AppCenter"));

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();


#if ANDROID || WINDOWS
        var appCenterOptions = app.Services.GetRequiredService<IOptions<AppCenterOptions>>();
        AppCenter.Start(appCenterOptions.Value.Secret, typeof(Analytics), typeof(Crashes));
#endif
        return app;
    }

    private static void ConfigureConfiguration(MauiAppBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.secrets.json", optional: false, false);
    }
}
