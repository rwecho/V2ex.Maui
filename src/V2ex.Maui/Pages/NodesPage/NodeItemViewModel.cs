using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

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