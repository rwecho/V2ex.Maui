using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class FollowingItemViewModel : ObservableObject
{
    public FollowingItemViewModel(FollowingInfo.ItemInfo item, NavigationManager navigationManager)
    {
        this.Avatar = item.Avatar;
        this.UserName = item.UserName;
        this.UserLink = item.UserLink;
        this.TopicTitle = item.TopicTitle;
        this.TopicLink = item.TopicLink;
        this.Replies = item.Replies;
        this.NodeLink = item.NodeLink;
        this.NodeName = item.NodeName;
        this.NodeId = item.NodeId;
        this.Created = item.Created;
        this.CreatedText = item.CreatedText;
        this.LastReplyUserName = item.LastReplyUserName;
        this.LastReplyUserLink = item.LastReplyUserLink;
        this.Id = item.Id;
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string _nodeId, _id, _avatar, _userName, _userLink, _topicTitle, _topicLink, _nodeName, _nodeLink;

    [ObservableProperty]
    private int _replies;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private string? _createdText, _lastReplyUserName, _lastReplyUserLink;

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task TapTitle(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            {TopicPageViewModel.QueryIdKey, this.Id }
        });
    }

    [RelayCommand]
    public async Task TapNode(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object>
        {
            {NodePageViewModel.QueryNodeKey, NodeId }
        });
    }

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            {MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
    }
}