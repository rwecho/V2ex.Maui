using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class NotificationsPage : ContentPage, ITransientDependency
{
	public NotificationsPage(NotificationsPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = this.ViewModel = viewModel;
	}

    private NotificationsPageViewModel ViewModel { get; }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
        base.OnNavigatedTo(args);

        if (this.ViewModel.CurrentState == StateKeys.Success)
		{
            return;
        }

        Task.Run(() => this.ViewModel!.Load());
    }

}