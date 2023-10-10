﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

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
    private bool _isLoggingIn;

    public bool CanLoginCommandExecute => !IsLoggingIn
        && !string.IsNullOrWhiteSpace(this.UserName)
        && !string.IsNullOrWhiteSpace(this.Password);

    [RelayCommand(CanExecute = nameof(CanLoginCommandExecute))]
    public async Task Login(CancellationToken cancellationToken = default)
    {
        try
        {
            this.IsLoggingIn = true;

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
            this.IsLoggingIn = false;
        }
    }

    [RelayCommand]
    public async Task RefreshCaptcha(CancellationToken cancellationToken = default)
    {
        this.CaptchaImage = await this.ApiService.GetCaptchaImage(this.LoginParameters);
    }
}