using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using V2ex.Api;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace V2ex.Maui.AppShell;


[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpBlobStoringModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpLocalizationModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AppModule>("V2ex.Maui");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.DefaultResourceType = typeof(MauiResource);
            options.Resources
                .Add<MauiResource>("zh")
                .AddVirtualJson("/Localization/Resources/Maui");
        });

        context.Services.AddTransient((sp) => DeviceDisplay.Current);
        context.Services.AddTransient((sp) =>
        {
            return new Lazy<IDispatcher>(Application.Current!.Dispatcher);
        });

        var configuration = context.Services.GetConfiguration();
        Configure<AppCenterOptions>(configuration.GetSection("AppCenter"));

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = Path.Combine(FileSystem.AppDataDirectory, "Blobs");
                });
            });
        });

        context.Services.AddSingleton<ApiService>();

        // add http client and configure cookie handler
        context.Services.AddHttpClient("api", client =>
        {
        })
            .ConfigurePrimaryHttpMessageHandler((sp) =>
            {
                return new CookieHttpClientHandler(sp.GetRequiredService<MauiPreferences>());
            })
            .AddHttpMessageHandler((sp) =>
            {
                return new LoggingHttpMessageHandler(sp.GetRequiredService<ILogger<ApiService>>());
            });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);
        var routingManager = context.ServiceProvider.GetRequiredService<RoutingManager>();

        routingManager.Register(nameof(MainPage), typeof(MainPage));
        routingManager.Register(nameof(MemberPage), typeof(MemberPage));
        routingManager.Register(nameof(TabPage), typeof(TabPage));
        routingManager.Register(nameof(SettingsPage), typeof(SettingsPage));
        routingManager.Register(nameof(TopicPage), typeof(TopicPage));
        routingManager.Register(nameof(MyNodesPage), typeof(MyNodesPage), true);
        routingManager.Register(nameof(MyTopicsPage), typeof(MyTopicsPage), true);
        routingManager.Register(nameof(MyFollowingPage), typeof(MyFollowingPage), true);
        routingManager.Register(nameof(NodePage), typeof(NodePage));
        routingManager.Register(nameof(NotificationsPage), typeof(NotificationsPage), true);
        routingManager.Register(nameof(LoginPage), typeof(LoginPage));
        routingManager.Register(nameof(TagPage), typeof(TagPage));


        var localizer = context.ServiceProvider.GetRequiredService<IStringLocalizer<MauiResource>>();
        var a = localizer["HelloWorld"];
    }
}