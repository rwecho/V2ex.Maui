using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class SettingsPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public SettingsPageViewModel(ApiService apiService, UserManager userManager, NavigationManager navigationManager)
    {
        this.ApiService = apiService;
        this.UserManager = userManager;
        this.NavigationManager = navigationManager;
    }
    private ApiService ApiService { get; }
    private UserManager UserManager { get; }
    private NavigationManager NavigationManager { get; }

    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private Exception? exception;
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
      
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

    [RelayCommand]
    public async Task Logout(CancellationToken cancellationToken)
    {
        if (!await Shell.Current.DisplayAlert("提示", "退出登录", "确定", "取消"))
        {
            return;
        }

        try
        {
            this.CurrentState = StateKeys.Loading;
            this.UserManager.Logout();
            this.CurrentState = StateKeys.Success;

            await this.NavigationManager.GoToAsync($"../../");
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}