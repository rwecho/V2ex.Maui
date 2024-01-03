using Microsoft.AspNetCore.Components;

namespace V2ex.Blazor.Components;

public record TopicViewModel(
    string Id,
    string? NodeId,
    MarkupString Title,
    string? Link,
    string? Avatar,
    string? AvatarLink,
    string? UserName,
    string? UserLink,
    MarkupString? LastRepliedText,
    string? LastRepliedBy,
    string? NodeName,
    string? NodeLink,
    int Replies)
{

}