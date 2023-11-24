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
    string? Once,
    string Url
    )
{
    public int CurrentPage { get; protected set; } = 1;
    public int MaximumPage { get; protected set; } = 0;

    public List<ReplyViewModel> Replies { get; } = [];

    public void UpdatePage(int currentPage, int maximumPage, IReadOnlyList<ReplyViewModel> replies)
    {
        CurrentPage = currentPage;
        MaximumPage = maximumPage;
        Replies.AddRange(replies);
    }
}

public record SupplementViewModel(
    DateTime Created,
    string CreatedText,
    MarkupString? Content
    )
{ }

