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
        this.AllowAutoRedirect = false;
        this.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        this.Preferences = preference;

        var cookies = this.Preferences.Get(CookiesFileName, Array.Empty<Cookie>());
        foreach (var cookie in cookies)
        {
            this.CookieContainer.Add(cookie);
        }

#if DEBUG
        // add the cloud flare cookie, the cf_clearance cookie is required to bypass the cloud flare protection
        // we can get the cookie from the browser or POSTMAN
        this.CookieContainer.Add(new Cookie("cf_clearance", "P_UFJcVWLvAAnpiEnxbozvcLLdRJaFZMCMJ9EL6I9FY-1699942796-0-1-7084b968.1423aadb.2785e688-160.0.0", "/", "v2ex.com"));
#endif
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var fragment = request.RequestUri.Fragment;
        if (request.RequestUri?.LocalPath == "/signin"
            && request.Method == HttpMethod.Post
            && response.StatusCode == HttpStatusCode.Found)
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