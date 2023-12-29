using System.Net;
using V2ex.Api;

namespace V2ex.Blazor.Services;

public class CookieContainerService
{
    private const string CookiesFileName = "cookies.json";
    private const string UserKey = "user.json";

    public CookieContainerService(IPreferences preferences)
    {
        this.Preferences = preferences;
        this.Container = new CookieContainer();

        Initialize();
    }
    public CookieContainer Container { get; }

    public UserInfo? User { get; private set; }
    private IPreferences Preferences { get; }

    private void Initialize()
    {
        try
        {
            var cookies = Preferences.Get(CookiesFileName, Array.Empty<Cookie>());
            foreach (var cookie in cookies)
            {
                this.Container.Add(cookie);
            }

            this.User = Preferences.Get<UserInfo?>(UserKey, null);
        }
        catch (Exception)
        {
        }
    }

    public void Logout()
    {
        foreach (Cookie cookie in this.Container.GetAllCookies())
        {
            cookie.Expires = DateTime.Now.AddDays(-1);
        }
        this.User = null;
        Preferences.Set(CookiesFileName, Array.Empty<Cookie>());
        Preferences.Set<UserInfo?>(UserKey, null);
    }

    public void Login(UserInfo userInfo)
    {
        this.User = userInfo;
        var cookies = this.Container.GetAllCookies()
               .Cast<Cookie>()
               .Select(x => new { x.Name, x.Value, x.Domain, x.Path, x.Expires, x.Secure, x.HttpOnly })
               .ToArray();
        Preferences.Set(CookiesFileName, cookies);
        Preferences.Set<UserInfo?>(UserKey, userInfo);
    }
}
