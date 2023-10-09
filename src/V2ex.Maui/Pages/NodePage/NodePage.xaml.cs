using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class NodePage : ContentPage, ITransientDependency
{
	public NodePage(NodePageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = this.ViewModel = viewModel;
	}

    private NodePageViewModel ViewModel { get; }

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