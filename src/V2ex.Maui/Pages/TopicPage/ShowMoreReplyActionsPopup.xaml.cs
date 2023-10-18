using CommunityToolkit.Maui.Views;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class ShowMoreReplyActionsPopup : Popup, ITransientDependency
{
    public ShowMoreReplyActionsPopup(ReplyViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

    private void Copy_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void Reply_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void Ignore_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void GotoReplierPage_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}