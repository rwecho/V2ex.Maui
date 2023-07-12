using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace V2ex.Maui.Pages;

public partial class TabPage : ContentPage, ITransientDependency
{
    private TabPageViewModel ViewModel { get; }

    public TabPage(TabPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext =this.ViewModel = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }

        Task.Run(() => this.ViewModel!.Load());
    }
}