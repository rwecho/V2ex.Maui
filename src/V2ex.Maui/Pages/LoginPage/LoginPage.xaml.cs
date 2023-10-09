using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class LoginPage : ContentPage, ITransientDependency
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
	}

    private LoginPageViewModel ViewModel { get; }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Task.Run(() => this.ViewModel.Load());
    }
}