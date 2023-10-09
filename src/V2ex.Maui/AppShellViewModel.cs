using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui;

public partial class AppShellViewModel : ObservableObject, ITransientDependency
{
    [ObservableProperty]
    private bool _flyoutIsPresented;

    public NavigationManager NavigationManager { get; }

    public AppShellViewModel(NavigationManager navigationManager)
    {
        this.NavigationManager = navigationManager;
    }

    [RelayCommand]
    public async Task GotoSettingsPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(SettingsPage), true);
        this.FlyoutIsPresented = false;
    }

    [RelayCommand]
    public async Task GotoNotificationsPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NotificationsPage), true);
        this.FlyoutIsPresented = false;
    }

    [RelayCommand]
    public async Task GotoMyFavoritePage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MyTopicsPage), true);
        this.FlyoutIsPresented = false;
    }

    [RelayCommand]
    public async Task GotoDailyHotPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(DailyHotPage), true);
        this.FlyoutIsPresented = false;
    }

    [RelayCommand]
    public async Task GotoNodesPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodesPage), true);
        this.FlyoutIsPresented = false;
    }
}