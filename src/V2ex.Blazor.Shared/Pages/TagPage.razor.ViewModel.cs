using V2ex.Blazor.Components;

namespace V2ex.Blazor.Pages;
public record TagPageViewModel(int CurrentPage,
    int MaximumPage,
    List<TopicViewModel> Topics)
{

}