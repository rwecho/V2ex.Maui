using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TagPage : ContentPage, ITransientDependency
{
	public TagPage(TagPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = this.ViewModel = viewModel;
	}

    private TagPageViewModel ViewModel { get; }

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