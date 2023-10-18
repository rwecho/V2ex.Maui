using CommunityToolkit.Mvvm.Messaging.Messages;

namespace V2ex.Maui.Pages;

public class ShowReplyInputWithUserNameMessage : ValueChangedMessage<string>
{
    public ShowReplyInputWithUserNameMessage(string value) : base(value)
    {
    }
}
