using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TopicPage : ContentPage, ITransientDependency
{
    private TopicPageViewModel ViewModel { get; }
	public TopicPage(TopicPageViewModel viewModel)
	{
		InitializeComponent();
        this.ContentWebView.Navigated += ContentWebView_Navigated;

        this.BindingContext = this.ViewModel = viewModel;
	}

    private void ContentWebView_Navigated(object? sender, WebNavigatedEventArgs e)
    {
        this.ContentWebView.EvaluateJavaScriptAsync("document.documentElement.scrollHeight;")
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && double.TryParse(task.Result, out double contentHeight))
                {
                    if(contentHeight == 1195)
                    {
                        return;
                    }
                    this.ContentWebView.HeightRequest = contentHeight;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if(this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel!.Load());
    }
}