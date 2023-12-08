using Microsoft.AspNetCore.Components;
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
    MarkupString? Content,
    List<SupplementViewModel> Supplements,
    string NodeName,
    string NodeLink,
    MarkupString? ReplyStats,
    List<string> Tags,
    string Url
    )
{
    public int CurrentPage { get; protected set; } = 1;
    public int MaximumPage { get; protected set; } = 0;

    public bool Liked { get;  set; }
    public bool Thanked { get;  set; }
    public bool Ignored { get;  set; }
    public string? Once { get; set; }
    public List<ReplyViewModel> Replies { get; } = [];

    public void UpdatePage(int currentPage, int maximumPage, IReadOnlyList<ReplyViewModel> replies)
    {
        CurrentPage = currentPage;
        MaximumPage = maximumPage;
        Replies.AddRange(replies);
    }
}

