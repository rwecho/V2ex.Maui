using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

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
