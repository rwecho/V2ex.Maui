using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

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
