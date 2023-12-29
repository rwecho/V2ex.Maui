using Microsoft.Extensions.Options;
using System.Net;

namespace V2ex.Blazor.Services;

public static class UserAgentConstants
{
#if WINDOWS
    public const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.0 Safari/605.1.15 Edg/120.0.0.0";
#elif ANDROID
    public const string UserAgent = "Mozilla/5.0 (Linux; Android 11; Pixel 5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.91 Mobile Safari/537.36";
#else
    public const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.0 Safari/605.1.15 Edg/120.0.0.0";
#endif
}

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
