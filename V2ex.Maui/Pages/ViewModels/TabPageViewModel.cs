using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TabPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string TagPageQueryKey = "tab";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TagPageQueryKey, out var tab))
        {
            TabName = tab.ToString()!;
        }
    }

    [ObservableProperty]
    private string _tabName = "all";
}