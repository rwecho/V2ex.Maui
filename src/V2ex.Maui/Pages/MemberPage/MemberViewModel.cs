using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;
using V2ex.Maui.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class MemberViewModel : ObservableObject
{
    public MemberViewModel(MemberPageInfo member)
    {
        this.UserName = member.UserName;
        this.Avatar = member.Avatar;
        this.Tagline = member.Tagline;
        this.Bank = member.TodayActivity;
        this.IsOnline = member.IsOnline != null;
        this.CreatedText = member.CreatedText;
        this.FollowOnClick = member.FollowOnClick;
        this.BlockOnClick = member.BlockOnClick;
        this.Topics = member.Topics.Select(o => TopicRowViewModel.Create(o.Title,
            "",
            o.UserName,
            o.CreatedText,
            o.NodeName,
            o.LatestReplyBy,
            Utilities.ParseId(o.Link),
            Utilities.ParseId(o.NodeLink),
            o.Replies
            )).ToList();
        this.Replies = member.Replies.Zip(member.ReplyContents)
            .Select(o => new MemberPageReplyViewModel(o.First, o.Second))
            .ToList();
    }

    [ObservableProperty]
    private string _userName, _avatar;

    [ObservableProperty]
    private string? _tagline, _bank, _createdText, _followOnClick, _blockOnClick;

    [ObservableProperty]
    private bool _isOnline;

    [ObservableProperty]
    private List<TopicRowViewModel> _topics;

    [ObservableProperty]
    private List<MemberPageReplyViewModel> _replies;
}
