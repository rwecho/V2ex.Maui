using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.AppShell.Components;

public partial class FlyoutHeaderViewModel: ObservableObject, ITransientDependency
{
    public FlyoutHeaderViewModel(UserManager userManager)
    {
        this.UserManager = userManager;

        this.User = this.UserManager.CurrentUser;

        this.UserManager.UserChanged += (sender, args) =>
        {
            this.User = this.UserManager.CurrentUser;
        };
    }

    private UserManager UserManager { get; }

    [ObservableProperty]
    private UserInfo? _user;
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