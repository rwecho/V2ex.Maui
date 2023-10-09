using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class NodeCategoryViewModel : ObservableObject
{
    public NodeCategoryViewModel(NodesNavInfo.ItemInfo nodeCategory)
    {
        this.Category = nodeCategory.Category;
        this.Items = nodeCategory.Nodes
            .Select(o => InstanceActivator.Create<NodeItemViewModel>(o))
            .ToList();
    }

    [ObservableProperty]
    private string _category;

    [ObservableProperty]
    private List<NodeItemViewModel> _items;
}
