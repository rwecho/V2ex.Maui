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
int Floor,
string? Thanked,
string? AlreadyThanked
);