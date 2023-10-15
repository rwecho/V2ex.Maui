using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
    }

    private void CallOutRepliesHandler(object recipient, CallOutRepliesMessage message)
    {
        this.ShowPopup(InstanceActivator.Create<RepliesPopup>(message.Value));
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

public class CallOutRepliesMessage : ValueChangedMessage<IReadOnlyList<ReplyViewModel>>
{
    public CallOutRepliesMessage(IReadOnlyList<ReplyViewModel> value) 
        : base(value)
    {
    }
}