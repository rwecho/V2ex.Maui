using CommunityToolkit.Maui.Alerts;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class ThemeSettingsViewModel : BaseViewModel
{
    private NightMode? _nightMode;

    public ThemeSettingsViewModel(AppPreferencesManager appPreferencesManager)
    {
        this.AppPreferencesManager = appPreferencesManager;
    }

    public NightMode? NightMode
    {
        get
        {
            return _nightMode;
        }
        set
        {
            if (value != null && this.SetProperty(ref _nightMode, value))
            {
                this.SetNightMode(value.Value);
            }
        }
    }

    private void SetNightMode(NightMode nightMode)
    {
        var appPreferences = this.AppPreferencesManager.Current ?? new();
        appPreferences.NightMode = nightMode;
        this.AppPreferencesManager.Set(appPreferences);
        var _ = Toast.Make("App theme changed success.").Show();

        if (nightMode == Services.NightMode.Open)
        {
            Application.Current!.UserAppTheme = AppTheme.Dark;
        }
        else if (nightMode == Services.NightMode.Close)
        {
            Application.Current!.UserAppTheme = AppTheme.Light;
        }
        else if (nightMode == Services.NightMode.FollowBySystem)
        {
            Application.Current!.UserAppTheme = AppTheme.Unspecified;
        }
    }

    private AppPreferencesManager AppPreferencesManager { get; }

    protected override Task OnLoad(CancellationToken cancellationToken)
    {
        _nightMode = this.AppPreferencesManager.Current?.NightMode ?? Services.NightMode.Close;
        OnPropertyChanged(nameof(NightMode));
        return Task.CompletedTask;
    }
}