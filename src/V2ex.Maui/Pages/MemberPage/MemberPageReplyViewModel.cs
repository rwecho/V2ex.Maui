using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;

namespace V2ex.Maui.Pages;

public partial class MemberPageReplyViewModel : ObservableObject
{
    public MemberPageReplyViewModel(MemberPageInfo.ReplyInfo reply, string content)
    {
        this.Content = content;
        this.OpUserName = reply.OpUserName;
        this.NodeName = reply.NodeName;
        this.TopicTitle = reply.TopicTitle;
        this.TopicLink = reply.TopicLink;
        this.ReplyTime = reply.ReplyTime;
        this.ReplyTimeText = reply.ReplyTimeText;
    }

    [ObservableProperty]
    private string _content, _replyTimeText, _opUserName, _nodeName, _topicTitle, _topicLink;

    [ObservableProperty]
    private DateTime _replyTime;
}