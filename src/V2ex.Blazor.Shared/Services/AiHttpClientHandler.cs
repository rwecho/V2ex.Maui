using System.Net;
using System.Runtime.Versioning;
using V2ex.Api;

namespace V2ex.Blazor.Services;

public class AiHttpClientHandler: HttpClientHandler
{
    private const string CookiesFileName = "cookies.json";

    [UnsupportedOSPlatform("browser")]
    public AiHttpClientHandler(IPreferences preferences)
    {
        this.CookieContainer = new CookieContainer();
        this.UseCookies = true;
        this.UseDefaultCredentials = false;
        this.AllowAutoRedirect = false;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        this.Preferences = preferences;

        var cookies = this.Preferences.Get(CookiesFileName, Array.Empty<Cookie>());
        foreach (var cookie in cookies)
        {
            cookie.Domain = "v2ex-maui.vercel.app";
            this.CookieContainer.Add(cookie);
        }
    }

    private IPreferences Preferences { get; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
    }
}