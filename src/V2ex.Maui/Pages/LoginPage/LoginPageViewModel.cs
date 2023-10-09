using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class LoginPageViewModel : BaseViewModel, IQueryAttributable
{
    private LoginViewModel? _login;
    private const string QueryNextKey = "next";

    [ObservableProperty]
    private string?  _next;

    [DisablePropertyInjection]
    public LoginViewModel? Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.TryGetValue(QueryNextKey, out var value))
        {
            this.Next = value.ToString();
        }
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        var loginParameters = await this.ApiService.GetLoginParameters();

        var captcha = await this.ApiService.GetCaptchaImage(loginParameters);
        this.Login = InstanceActivator.Create<LoginViewModel>(loginParameters, captcha, this.Next ?? "../../");
    }
}
