namespace V2ex.Blazor.Components;

public record TopicViewModel(
    string Id,
    string NodeId,
    string Title,
    string Link,
    string Avatar,
    string AvatarLink,
    string UserName,
    string UserLink,
    DateTime LastReplied,
    string? LastRepliedText,
    string? LastRepliedBy,
    string NodeName,
    string NodeLink,
    int Replies)
{

}