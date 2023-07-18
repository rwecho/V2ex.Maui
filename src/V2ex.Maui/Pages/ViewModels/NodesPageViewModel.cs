using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NodesPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState;
    [ObservableProperty]
    private Exception? _exception;
    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private List<NodeViewModel>? _nodes;

    public NodesPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
        
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            var nodeInfo = await this.ApiService.GetFavoriteNodes() ?? throw new InvalidOperationException("获取节点失败");
            this.Nodes = nodeInfo.Items
                .Select(o=> new NodeViewModel(o))
                .ToList();
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class NodeViewModel : ObservableObject
{
    public NodeViewModel(FavoriteNodeInfo.ItemInfo node)
    {
        this.Image = node.Image;
        this.Name = node.Name;
        this.Topics = node.Topics;
        this.Link = node.Link;
    }

    [ObservableProperty]
    private string _image,_name, _topics, _link;
}