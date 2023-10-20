using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class SettingsPageViewModel : BaseViewModel, IQueryAttributable
{
    public SettingsPageViewModel(UserManager userManager, NavigationManager navigationManager, 
        AppPreferencesManager appPreferencesManager)
    {
        this.UserManager = userManager;
        this.NavigationManager = navigationManager;
        this.AppPreferencesManager = appPreferencesManager;
        this.AppPreferencesManager.CurrentChanged += this.AppPreferencesManager_CurrentChanged;
    }

    private UserManager UserManager { get; }
    private NavigationManager NavigationManager { get; }
    private AppPreferencesManager AppPreferencesManager { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NightModeText))]
    private AppPreferences? _appPreferences;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [RelayCommand]
    public async Task Logout(CancellationToken cancellationToken)
    {
        if (!await Shell.Current.DisplayAlert(
            this.Localizer["AlertTitle"],
            this.Localizer["LogoutConfirmMessage"],
            this.Localizer["Ok"],
            this.Localizer["Cancel"]))
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

    [RelayCommand]
    public async Task GotoThemePage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(ThemeSettingsPage), true);
    }

    [RelayCommand]
    public async Task GotoHomeSettingPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(HomeSettingPage), true);
    }

    protected override Task OnLoad(CancellationToken cancellationToken)
    {
        this.AppPreferences = this.AppPreferencesManager.Current;
        return Task.CompletedTask;
    }

    private void AppPreferencesManager_CurrentChanged(object? sender, EventArgs e)
    {
        this.OnPropertyChanged(nameof(NightModeText));
    }

    public string NightModeText
    {
        get
        {
            var nightMode = this.AppPreferences?.NightMode ?? NightMode.Close;
            return nightMode switch
            {
                NightMode.Open => this.Localizer["Open"],
                NightMode.Close => this.Localizer["Close"],
                NightMode.FollowBySystem => this.Localizer["FollowBySystem"],
                _ => throw new NotSupportedException(),
            };
        }
    }
}