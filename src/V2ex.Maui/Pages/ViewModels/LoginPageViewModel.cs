using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;


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

public partial class LoginViewModel : ObservableObject, ITransientDependency
{
    public LoginViewModel(LoginParameters loginParameters,
        byte[] captchaImage,
        string next,
        ApiService apiService,
        UserManager userManager,
        NavigationManager navigationManager)
    {
        this.Next = next;
        this.LoginParameters = loginParameters;
        this.CaptchaImage = captchaImage;
        this.ApiService = apiService;
        this.UserManager = userManager;
        this.NavigationManager = navigationManager;
    }


    [ObservableProperty]
    private byte[] _captchaImage;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _userName, _password, _captcha;
    [ObservableProperty]
    private string _next;


    private LoginParameters LoginParameters { get; }
    private ApiService ApiService { get; }
    private UserManager UserManager { get; }
    private NavigationManager NavigationManager { get; }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isLogining;

    public bool CanLoginCommandExecute => !IsLogining
        && !string.IsNullOrWhiteSpace(this.UserName)
        && !string.IsNullOrWhiteSpace(this.Password);

    [RelayCommand(CanExecute = nameof(CanLoginCommandExecute))]
    public async Task Login(CancellationToken cancellationToken = default)
    {
        try
        {
            this.IsLogining = true;

            if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.Captcha))
            {
                return;
            }

            var newsInfo = await this.ApiService.Login(
                this.LoginParameters,
                this.UserName,
                this.Password,
                this.Captcha
                );

            if (newsInfo.CurrentUser != null)
            {
                this.UserManager.Login(newsInfo.CurrentUser);
            }

            await this.NavigationManager.GoToAsync(this.Next);
        }
        catch (InvalidOperationException exception)
        {
            await Shell.Current.DisplayAlert("Login Error", exception.Message, "OK");
        }
        finally
        {
            this.IsLogining = false;
        }
    }

    [RelayCommand]
    public async Task RefreshCaptcha(CancellationToken cancellationToken = default)
    {
        this.CaptchaImage = await this.ApiService.GetCaptchaImage(this.LoginParameters);
    }
}