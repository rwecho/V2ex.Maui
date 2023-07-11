using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
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
            var handler = new CookieHttpClientHandler
            {
                
            };
            return new HttpClient(handler);
        });
    }
}

public class CookieHttpClientHandler : HttpClientHandler
{
    public CookieHttpClientHandler()
    {
        this.CookieContainer = new CookieContainer();

        // load cookies from cookies.json
        if (File.Exists("cookies.json"))
        {
            var json = File.ReadAllText("cookies.json");
            var cookies = JsonSerializer.Deserialize<Cookie[]>(json);
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    this.CookieContainer.Add(cookie);
                }
            }
        }

        this.UseCookies = true;
        this.UseDefaultCredentials = false;
        this.AllowAutoRedirect = true;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (request.RequestUri?.LocalPath == "/signin" 
            && request.Method == HttpMethod.Post
            && response.IsSuccessStatusCode)
        {
            // serialize cookies to json, save to cookies.json
            var cookies = this.CookieContainer.GetAllCookies()
                .Cast<Cookie>()
                .Select(x => new { x.Name, x.Value, x.Domain, x.Path, x.Expires, x.Secure, x.HttpOnly })
                .ToArray();
            var json = JsonSerializer.Serialize(cookies, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("cookies.json", json);
        }
        return response;
    }
}
