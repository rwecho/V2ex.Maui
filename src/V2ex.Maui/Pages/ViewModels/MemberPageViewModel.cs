using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class MemberPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryUserNameKey = "username";

    [ObservableProperty]
    private string?  _userName;

    [ObservableProperty]
    private MemberViewModel? _member;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryUserNameKey, out var username))
        {
            this.UserName = username.ToString();
        }
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.UserName))
        {
            throw new InvalidOperationException("User name can not not be empty.");
        }

        var memberInfo = await this.ApiService.GetUserPageInfo(this.UserName);
        this.Member = memberInfo == null ? null : new MemberViewModel(memberInfo);
    }
}

public partial class MemberViewModel: ObservableObject
{
    public MemberViewModel(MemberPageInfo member)
    {
        this.UserName = member.UserName;
        this.Avatar = member.Avatar;
        this.Tagline = member.Tagline;
        this.Bank = member.Bank;
        this.CreatedText = member.CreatedText;
        this.FollowOnClick = member.FollowOnClick;
        this.BlockOnClick = member.BlockOnClick;
        this.Topics = member.Topics.Select(o=> new MemberPageTopicViewModel(o)).ToList();
        this.Replies = member.Replies.Zip(member.ReplyContents)
            .Select(o=> new MemberPageReplyViewModel(o.First,o.Second))
            .ToList();
    }

    [ObservableProperty]
    private string _userName, _avatar;
    [ObservableProperty]
    private string? _tagline, _bank, _createdText, _followOnClick, _blockOnClick;


    [ObservableProperty]
    private List<MemberPageTopicViewModel> _topics;

    [ObservableProperty]
    private List<MemberPageReplyViewModel> _replies;
}

public partial class MemberPageTopicViewModel: ObservableObject
{
    public MemberPageTopicViewModel(MemberPageInfo.TopicInfo topic)
    {
        this.Title = topic.Title;
        this.UserName = topic.UserName;
        this.LatestReplyUserName = topic.LatestReplyUserName;
        this.NodeName = topic.NodeName;
        this.NodeLink = topic.NodeLink;
        this.CreatedText = topic.CreatedText;
        this.Created = topic.Created;
        this.Link = topic.Link;
        this.ReplyNumber = topic.ReplyNumber;
    }

    [ObservableProperty]
    private string _title,_link, _userName, _nodeName, _nodeLink;

    [ObservableProperty]
    private string? _LatestReplyUserName, _createdText;
    [ObservableProperty]
    private int _replyNumber;
    [ObservableProperty]
    private DateTime _created;
}

public partial class MemberPageReplyViewModel: ObservableObject
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