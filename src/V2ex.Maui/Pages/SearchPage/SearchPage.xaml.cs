using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class SearchPage : ContentPage, ITransientDependency
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
    }

    public SearchPageViewModel ViewModel { get; }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Task.Run(() => this.ViewModel!.Load());

    }
}
