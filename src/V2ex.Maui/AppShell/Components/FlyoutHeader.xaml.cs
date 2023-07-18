using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.AppShell.Components;

public partial class FlyoutHeaderViewModel: ObservableObject, ITransientDependency
{
    public FlyoutHeaderViewModel(UserManager userManager, NavigationManager navigationManager)
    {
        this.UserManager = userManager;
        this.NavigationManager = navigationManager;
        this.User = this.UserManager.CurrentUser;

        this.UserManager.UserChanged += (sender, args) =>
        {
            this.User = this.UserManager.CurrentUser;
        };
    }

    private UserManager UserManager { get; }
    private NavigationManager NavigationManager { get; }

    [ObservableProperty]
    private UserInfo? _user;

    [RelayCommand]
    public async Task GotoNodes(CancellationToken cancellationToken = default)
    {
        await this.NavigationManager.GoToAsync(nameof(MyNodesPage));
    }

    [RelayCommand]
    public async Task GotoTopics(CancellationToken cancellationToken = default)
    {
        await this.NavigationManager.GoToAsync(nameof(MyTopicsPage));
    }

    [RelayCommand]
    public async Task GotoFollowing(CancellationToken cancellationToken = default)
    {
        await this.NavigationManager.GoToAsync(nameof(MyFollowingPage));
    }

    [RelayCommand]
    public async Task GotoLogin(CancellationToken cancellationToken = default)
    {
        await this.NavigationManager.GoToAsync(nameof(LoginPage));
    }
}

public partial class FlyoutHeader : ContentView, ITransientDependency
{
	public FlyoutHeader(FlyoutHeaderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = this.ViewModel = viewModel;
	}

    private FlyoutHeaderViewModel ViewModel { get; }
}