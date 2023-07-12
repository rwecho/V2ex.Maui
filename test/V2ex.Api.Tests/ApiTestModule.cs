using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace V2ex.Api.Tests;

[DependsOn(typeof(AbpTestBaseModule))]
public class ApiTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<ApiService>();
        context.Services.AddSingleton((sp) =>
        {
            var handler = new CookieHttpClientHandler(sp.GetRequiredService<FilePreferences>());
            return new HttpClient(handler);
        });
    }
}