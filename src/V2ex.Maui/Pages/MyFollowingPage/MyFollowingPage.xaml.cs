using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class MyFollowingPage : ContentPage, ITransientDependency
{
	public MyFollowingPage(MyFollowingPageViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
	}

    private MyFollowingPageViewModel ViewModel { get; }

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