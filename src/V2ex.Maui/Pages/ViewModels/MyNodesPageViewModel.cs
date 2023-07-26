using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class MyNodesPageViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private List<MyNodesItemViewModel>? _nodes;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        var nodeInfo = await this.ApiService.GetFavoriteNodes() ?? throw new InvalidOperationException("获取节点失败");
        this.Nodes = nodeInfo.Items
            .Select(o => InstanceActivator.Create<MyNodesItemViewModel>(o))
            .ToList();
    }
}

public partial class MyNodesItemViewModel : ObservableObject
{
    public MyNodesItemViewModel(FavoriteNodeInfo.ItemInfo node,
        NavigationManager navigationManager)
    {
        this.Image = node.Image;
        this.Name = node.Name;
        this.Topics = node.Topics;
        this.Link = node.Link;
        this.Id = Utilities.ParseId(node.Link);
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string _id, _image, _name, _topics, _link;

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task GotoNode(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object> { { NodePageViewModel.QueryNodeKey, Id } });
    }
}