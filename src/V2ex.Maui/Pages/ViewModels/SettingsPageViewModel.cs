using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel, IQueryAttributable
{
    public SettingsPageViewModel(UserManager userManager, NavigationManager navigationManager)
    {
        this.UserManager = userManager;
        this.NavigationManager = navigationManager;
    }

    private UserManager UserManager { get; }
    private NavigationManager NavigationManager { get; }

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

    protected override Task OnLoad(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}