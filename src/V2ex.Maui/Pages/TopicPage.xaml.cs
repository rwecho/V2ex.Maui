using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TopicPage : ContentPage, ITransientDependency
{
	public TopicPage(TopicPageViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var viewModel = this.BindingContext as TopicPageViewModel;

        Task.Run(() => viewModel!.Load());
    }
}