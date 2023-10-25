using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
#if ! IOS
using SByteDev.Serilog.Sinks.AppCenter;
#endif
using Serilog;
using Serilog.Events;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace V2ex.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp(string? filesDir = null)
    {
        var builder = MauiApp.CreateBuilder();
        SetupSerilog(filesDir);
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("apple-logo.ttf", "Apple");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureContainer(new AbpAutofacServiceProviderFactory(new Autofac.ContainerBuilder()));

        builder.Logging.AddSerilog(dispose: true);
        ConfigureConfiguration(builder);

        builder.Services.AddApplication<AppModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
        });

        var app = builder.Build();

        app.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(app.Services);

        return app;
    }

    private static void SetupSerilog(string? filesDir = null)
    {
        var logFilePath = Path.Combine(filesDir ?? string.Empty, "log.txt");
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
#if ! IOS
        .WriteTo.AppCenter(LogEventLevel.Information)
#endif
        .WriteTo.File(logFilePath)
        .CreateLogger();
    }

    private static void ConfigureConfiguration(MauiAppBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.secrets.json", optional: false, false);
    }
}