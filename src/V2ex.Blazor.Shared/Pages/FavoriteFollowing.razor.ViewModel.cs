using V2ex.Blazor.Components;
namespace V2ex.Blazor.Pages;
public record FollowingPageViewModel(int CurrentPage, int MaximumPage, IReadOnlyList<TopicViewModel> Topics);
