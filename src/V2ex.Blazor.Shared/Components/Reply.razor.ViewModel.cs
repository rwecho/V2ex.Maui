using Microsoft.AspNetCore.Components;

namespace V2ex.Blazor.Components;
public record ReplyViewModel(string Id,
    MarkupString? Content,
    string UserName,
    string UserLink,
    string Avatar,
    DateTime ReplyTime,
    string ReplyTimeText,
    string? Badges,
    int Floor
)
{
    public string? Once { get; set; }

    public bool Thanked { get; set; }

    public int AlreadyThanked { get; set; }
};