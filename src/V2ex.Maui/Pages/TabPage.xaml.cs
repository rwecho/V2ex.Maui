using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace V2ex.Maui.Pages;

public partial class TabPage : ContentPage, ITransientDependency
{
	public TabPage(TabPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var viewModel = this.BindingContext as TabPageViewModel;

        Task.Run(() => viewModel!.Load());
    }
}