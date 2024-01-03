using Microsoft.AspNetCore.Components;
using System.Data;
using V2ex.Api;
using V2ex.Blazor.Components;

namespace V2ex.Blazor.Pages;
public record TabViewModel(NewsInfo NewsInfo)
{
    public UserInfo? CurrentUser => NewsInfo.CurrentUser;

    public IReadOnlyList<TopicViewModel> Topics => NewsInfo.Items
    .Select(x => new TopicViewModel(
        x.Id,
        x.NodeId,
        new MarkupString(x.Title),
        x.Link,
        x.Avatar,
        x.AvatarLink,
        x.UserName,
        x.UserLink,
        x.LastRepliedText == null? null: new MarkupString(x.LastRepliedText),
        x.LastRepliedBy,
        x.NodeName,
        x.NodeLink,
        x.Replies)).ToList();
}