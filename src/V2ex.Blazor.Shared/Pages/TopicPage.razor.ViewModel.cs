using Microsoft.AspNetCore.Components;
using V2ex.Api;
using V2ex.Blazor.Components;

namespace V2ex.Blazor.Pages;

public record TopicPageViewModel(
    string NodeId,
    string Title,
    string UserName,
    string UserLink,
    string Avatar,
    DateTime Created,
    string CreatedText,
    string? TopicStats,
    string? Liked,
    string? Thanked,
    MarkupString? Content,
    List<SupplementViewModel> Supplements,
    string NodeName,
    string NodeLink,
    MarkupString? ReplyStats,
    List<string> Tags,
    int CurrentPage,
    int MaximumPage,
    List<ReplyViewModel> Replies,
    string? Once,
    string Url
    )
{
}

public record SupplementViewModel(
    DateTime Created,
    string CreatedText,
    MarkupString? Content
    )
{ }

