using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NodesPageViewModel : BaseViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [ObservableProperty]
    private List<NodeCategoryViewModel>? _nodeCategories;

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        var nodesNavInfo = await this.ApiService.GetNodesNavInfo();

        this.NodeCategories = nodesNavInfo.Items
            .Select(o => InstanceActivator.Create<NodeCategoryViewModel>(o))
            .ToList();
    }
}

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

public partial class NodeItemViewModel : ObservableObject
{
    public NodeItemViewModel(NodesNavInfo.ItemInfo.NodeItemInfo node,
        NavigationManager navigationManager)
    {
        this.Name = node.Name;
        this.Link = node.Link;
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string _name, _link;

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task GotoNodePage(CancellationToken cancellationToken = default)
    {
        var node = new UriBuilder(UrlUtilities.CompleteUrl(this.Link)).Path.Split('/').Last();

        await this.NavigationManager.GoToAsync(nameof(NodePage), true,
             new Dictionary<string, object>
             {
                { NodePageViewModel.QueryNodeKey, node }
             });
    }
}