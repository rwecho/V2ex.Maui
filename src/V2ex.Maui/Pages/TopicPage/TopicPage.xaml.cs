using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;


public partial class TopicPage : ContentPage, ITransientDependency
{
    private TopicPageViewModel ViewModel { get; }
    public TopicPage(TopicPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
        WeakReferenceMessenger.Default.Register<CallOutRepliesMessage>(this, this.CallOutRepliesHandler);
        WeakReferenceMessenger.Default.Register<ShowMoreReplyActionsMessage>(this, this.ShowMoreReplyActionsHandler);
        WeakReferenceMessenger.Default.Register<ShowReplyInputMessage>(this, this.ShowReplyInputHandler);
    }

    private void ShowReplyInputHandler(object recipient, ShowReplyInputMessage message)
    {
        this.ShowPopup(InstanceActivator.Create<ReplyPopup>(message.Value));
    }

    private void CallOutRepliesHandler(object recipient, CallOutRepliesMessage message)
    {
        this.ShowPopup(InstanceActivator.Create<RepliesPopup>(message.Value));
    }

    private void ShowMoreReplyActionsHandler(object recipient, ShowMoreReplyActionsMessage message)
    {
        var popup = InstanceActivator.Create<ShowMoreReplyActionsPopup>(message.Value);
        this.ShowPopup(popup);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel!.Load());
    }
}
