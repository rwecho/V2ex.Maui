using V2ex.Blazor.Components;
namespace V2ex.Blazor.Pages;
public record FavoriteTopicsPageViewModel(int CurrentPage, int MaximumPage, IReadOnlyList<TopicViewModel> Topics);
