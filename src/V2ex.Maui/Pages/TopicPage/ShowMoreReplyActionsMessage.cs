using CommunityToolkit.Mvvm.Messaging.Messages;

namespace V2ex.Maui.Pages;

public class ShowMoreReplyActionsMessage : ValueChangedMessage<ReplyViewModel>
{
    public ShowMoreReplyActionsMessage(ReplyViewModel replyViewModel) : base(replyViewModel)
    {
    }
}