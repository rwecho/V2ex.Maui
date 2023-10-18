using CommunityToolkit.Mvvm.Messaging.Messages;

namespace V2ex.Maui.Pages;

public class CallOutRepliesMessage : ValueChangedMessage<IReadOnlyList<ReplyViewModel>>
{
    public CallOutRepliesMessage(IReadOnlyList<ReplyViewModel> value) 
        : base(value)
    {
    }
}
