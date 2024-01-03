using System.Net;
using V2ex.Api;
using V2ex.Blazor.Services;

namespace V2ex.Blazor.Pages;

public partial class LoginWithGooglePage : ContentPage
{
	public LoginWithGooglePage(string once, Action<bool> loginCallback)
	{
		InitializeComponent();
        this.BindingContext = this.ViewModel = 
            InstanceCreator.Create<LoginWithGooglePageViewModel>();
        this.ViewModel.Once = once;

        this.CookieContainerService = InstanceCreator.Create<CookieContainerService>();
        this.LoginCallback = loginCallback;

        this.WebView.UserAgent = this.ViewModel.UserAgent;

#if !WINDOWS
        // The app crashes when the WebView tries to access the cookie container on Windows.
        this.WebView.Cookies = this.CookieContainerService.Container;
#endif
    }

    private LoginWithGooglePageViewModel ViewModel { get; }
    private CookieContainerService CookieContainerService { get; }
    private Action<bool> LoginCallback { get; }

    private bool isAuthenticated;

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith($"{UrlUtilities.BASE_URL}/auth/google?code"))
        {
            isAuthenticated = true;
        }
        else if (e.Url == $"{UrlUtilities.BASE_URL}/" && isAuthenticated)
        {
            this.Dispatcher.DispatchAsync(async () =>
            {
                await ExchangeCookies();
                await this.Navigation.PopAsync();
                this.LoginCallback(true);
            });
        }

        if(e.Url.StartsWith($"{UrlUtilities.BASE_URL}/auth/google?once"))
        {
            if(this.WebView.Cookies!=null)
            {
                var cookies = this.WebView.Cookies.GetAllCookies();
            }
        }
    }

    private async Task ExchangeCookies()
    {
#if WINDOWS
        var webView = this.WebView.Handler!.PlatformView as Microsoft.UI.Xaml.Controls.WebView2;
        var cookieManager = webView!.CoreWebView2.CookieManager;

        foreach (var webView2Cookie in await cookieManager.GetCookiesAsync(UrlUtilities.BASE_URL))
        {
            var cookie = new Cookie(webView2Cookie.Name, webView2Cookie.Value, webView2Cookie.Path, webView2Cookie.Domain);

            this.CookieContainerService.Container.Add(cookie);
        }
#else
        await Task.CompletedTask;
#endif        
    }

#if WINDOWS
    protected override async void OnHandlerChanged()
    {

        if(this.WebView.Handler == null)
        {
            throw new Exception("WebView.Handler is null");
        }

        var webView = this.WebView.Handler.PlatformView as Microsoft.UI.Xaml.Controls.WebView2;

        if(webView == null)
        {
            throw  new Exception("WebView.Handler.PlatformView is null");
        }

        await webView.EnsureCoreWebView2Async();
        var cookieManager = webView.CoreWebView2.CookieManager;

        foreach (Cookie cookie in CookieContainerService.Container.GetAllCookies())
        {
            var cookie2 = cookieManager.CreateCookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path);
            cookieManager.AddOrUpdateCookie(cookie2);
        }

    }
#endif

    protected override bool OnBackButtonPressed()
    {
        LoginCallback(false);
        return base.OnBackButtonPressed();
    }
}
