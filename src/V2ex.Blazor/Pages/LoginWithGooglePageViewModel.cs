using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using V2ex.Api;
using V2ex.Blazor.Services;

namespace V2ex.Blazor.Pages;

public partial class LoginWithGooglePageViewModel: ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Url))]
    private string? userAgent, once;

    [ObservableProperty]
    private CookieContainer? cookies;

    [ObservableProperty]
    private bool hasNavigationBar = DeviceInfo.Platform != DevicePlatform.WinUI;

    public string Url
    {
        get
        {
            if(string.IsNullOrEmpty(this.Once))
            {
                return string.Empty;
            }
            return $"{UrlUtilities.BASE_URL}/auth/google?once={this.Once}";
        }
    }

    public LoginWithGooglePageViewModel()
    {
#if ANDROID
        this.UserAgent = UserAgentConstants.UserAgent;
#else
        this.UserAgent = UserAgentConstants.UserAgent;
#endif
    }
}