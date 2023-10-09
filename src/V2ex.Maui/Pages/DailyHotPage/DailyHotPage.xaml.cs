using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class DailyHotPage : ContentPage, ITransientDependency
{
    public DailyHotPage(DailyHotPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
    }

    private DailyHotPageViewModel ViewModel { get; }

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