using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using V2ex.Blazor;

namespace V2ex.Api.Tests;

[DependsOn(typeof(AbpTestBaseModule))]
public class ApiTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddBlazorShared();

        context.Services.AddSingleton<ApiService>();
        //context.Services.AddHttpClient("api", client =>
        //    {
        //    }).ConfigurePrimaryHttpMessageHandler((sp) =>
        //    {
        //        return new CookieHttpClientHandler(sp.GetRequiredService<FilePreferences>());
        //    });
    }
}