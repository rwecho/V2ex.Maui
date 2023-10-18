using CommunityToolkit.Maui.Views;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class ReplyPopup : Popup, ITransientDependency
{
    public ReplyPopup(TopicViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
        this.ViewModel.IsInputting = true;
    }

    private TopicViewModel ViewModel { get; }

    protected override Task OnClosed(object? result, bool wasDismissedByTappingOutsideOfPopup)
    {
        this.ViewModel.IsInputting = false;
        return base.OnClosed(result, wasDismissedByTappingOutsideOfPopup);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}