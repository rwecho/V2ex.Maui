using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TopicPage : ContentPage, ITransientDependency
{
    private TopicPageViewModel ViewModel { get; }
	public TopicPage(TopicPageViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if(this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel!.Load());
    }

    private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {

    }
}