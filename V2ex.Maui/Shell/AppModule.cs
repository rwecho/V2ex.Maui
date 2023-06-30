using V2ex.Maui.Pages;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace V2ex.Maui.Shell;

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

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(MemberPage), typeof(MemberPage));
        Routing.RegisterRoute(nameof(TabPage), typeof(TabPage));
        Routing.RegisterRoute(nameof(TopicPage), typeof(TopicPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(NodesPage), typeof(NodesPage));
        Routing.RegisterRoute(nameof(NodePage), typeof(NodePage));
    }
}
