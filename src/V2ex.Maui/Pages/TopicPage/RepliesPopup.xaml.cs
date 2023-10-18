using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class RepliesPopup : Popup, ITransientDependency
{
    public RepliesPopup(IReadOnlyList<ReplyViewModel> replies)
    {
        this.BindingContext = new RepliesPopupViewModel(replies);
        InitializeComponent();
    }
}
