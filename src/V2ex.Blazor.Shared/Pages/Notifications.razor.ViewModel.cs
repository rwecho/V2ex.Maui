namespace V2ex.Blazor.Pages;

public record NotificationsPageViewModel(int CurrentPage,
int MaximumPage,
int Total,
IReadOnlyList<NotificationItemViewModel> Items)
{

}

public record NotificationItemViewModel(string UserName,
string UserLink,
string Avatar,
string TopicLink,
string TopicTitle,
string? PreTitle,
string? PostTitle,
string? Created,
string? Payload,
string Id
);