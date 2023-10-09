using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

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