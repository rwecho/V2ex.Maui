using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace V2ex.Api;

public class CookieHttpClientHandler : HttpClientHandler
{
    private const string CookiesFileName = "cookies.json";
    private IPreferences Preferences { get; }

    public CookieHttpClientHandler(IPreferences preference)
    {
        this.CookieContainer = new CookieContainer();
        this.UseCookies = true;
        this.UseDefaultCredentials = false;
        this.AllowAutoRedirect = true;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        this.Preferences = preference;

        var cookies = this.Preferences.Get(CookiesFileName, Array.Empty<Cookie>());
        foreach (var cookie in cookies)
        {
            this.CookieContainer.Add(cookie);
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (request.RequestUri?.LocalPath == "/signin"
            && request.Method == HttpMethod.Post
            && response.IsSuccessStatusCode)
        {
            // serialize cookies to json, save to cookies.json
            var cookies = this.CookieContainer.GetCookies(request.RequestUri)
                .Cast<Cookie>()
                .Select(x => new { x.Name, x.Value, x.Domain, x.Path, x.Expires, x.Secure, x.HttpOnly })
                .ToArray();
            this.Preferences.Set(CookiesFileName, cookies);
        }
        return response;
    }
}