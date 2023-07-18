using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;


[DisablePropertyInjection]
public partial class LoginPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    private const string QueryNextKey = "next";
    [ObservableProperty]
    private string? _currentState, _next;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private LoginViewModel? _login;

    public LoginPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.TryGetValue(QueryNextKey, out var value))
        {
            this.Next = value.ToString();
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            var loginParameters = await this.ApiService.GetLoginParameters();

            var captcha = await this.ApiService.GetCaptchaImage(loginParameters);
            this.Login = InstanceActivator.Create<LoginViewModel>(loginParameters, captcha, this.Next ?? "/");
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class LoginViewModel : ObservableObject, ITransientDependency
{
    public LoginViewModel(LoginParameters loginParameters,
        byte[] captchaImage,
        string? next,
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
    private string? _userName, _password, _captcha, _next;


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

            await this.NavigationManager.GoToAsync(this.Next ?? "/");
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