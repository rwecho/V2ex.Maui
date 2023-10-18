using CommunityToolkit.Mvvm.Messaging.Messages;

namespace V2ex.Maui.Pages;

public class ShowReplyInputMessage : ValueChangedMessage<TopicViewModel>
{
    public ShowReplyInputMessage(TopicViewModel value) : base(value)
    {
    }
}