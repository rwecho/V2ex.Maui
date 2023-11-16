using V2ex.Api;
using V2ex.Blazor.Components;

namespace V2ex.Blazor.Pages;

public record HomeViewModel(NewsInfo NewsInfo)
{
    public string? Url => NewsInfo.Url;

    public UserInfo? CurrentUser => NewsInfo.CurrentUser;

    public IReadOnlyList<TopicViewModel> Topics => NewsInfo.Items
    .Select(x => new TopicViewModel(
        x.Id,
        x.NodeId,
        x.Title,
        x.Link,
        x.Avatar,
        x.AvatarLink,
        x.UserName,
        x.UserLink,
        x.LastReplied,
        x.LastRepliedText,
        x.LastRepliedBy,
        x.NodeName,
        x.NodeLink,
        x.Replies)).ToList();
}