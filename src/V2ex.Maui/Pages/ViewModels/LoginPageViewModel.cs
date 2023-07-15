using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;


[DisablePropertyInjection]
public partial class LoginPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState;

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
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            var loginParameters = await this.ApiService.GetLoginParameters();

            var captcha = await this.ApiService.GetCaptchaImage(loginParameters);
            this.Login = InstanceActivator.Create<LoginViewModel>(loginParameters, captcha);
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
        ApiService apiService,
        UserManager userManager)
    {
        this.LoginParameters = loginParameters;
        this.NameParameter = this.LoginParameters.NameParameter;
        this.PasswordParameter = this.LoginParameters.PasswordParameter;
        this.Once = this.LoginParameters.Once;
        this.CaptchaParameter = this.LoginParameters.CaptchaParameter;
        this.CaptchaImage = captchaImage;
        this.ApiService = apiService;
        this.UserManager = userManager;
    }

    [ObservableProperty]
    private string _nameParameter, _passwordParameter, _once, _captchaParameter;

    [ObservableProperty]
    private byte[] _captchaImage;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _userName, _password, _captcha;


    private LoginParameters LoginParameters { get; }
    private ApiService ApiService { get; }
    private UserManager UserManager { get; }

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