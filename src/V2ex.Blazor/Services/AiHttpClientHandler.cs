using Microsoft.Extensions.Options;
using System.Net;

namespace V2ex.Blazor.Services;

public class ApiHttpClientHandler: HttpClientHandler
{
    public ApiHttpClientHandler(CookieContainerService cookieContainerService)
    {
        this.CookieContainer = cookieContainerService.Container;
        this.UseCookies = true;
        this.UseDefaultCredentials = false;
        this.AllowAutoRedirect = false;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
    }
}

public class AiHttpClientHandler : HttpClientHandler
{
    public AiHttpClientHandler(CookieContainerService cookieContainerService, IOptions<ChatGPTOptions> options)
    {
        this.CookieContainer = new CookieContainer();
        this.UseCookies = true;
        this.UseDefaultCredentials = false;
        this.AllowAutoRedirect = false;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        var endpoint = options.Value.Endpoint;
        var host = new Uri(endpoint).Host;

        foreach (Cookie cookie in cookieContainerService.Container.GetAllCookies())
        {
            cookie.Domain = host;
            this.CookieContainer.Add(cookie);
        }
    }
}
