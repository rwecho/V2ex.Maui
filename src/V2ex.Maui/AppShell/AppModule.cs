﻿using Autofac;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using V2ex.Api;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace V2ex.Maui.AppShell;

[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpBlobStoringModule),
    typeof(AbpBlobStoringFileSystemModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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
        routingManager.Register(nameof(LoginPage), typeof(LoginPage), true);
    }
}