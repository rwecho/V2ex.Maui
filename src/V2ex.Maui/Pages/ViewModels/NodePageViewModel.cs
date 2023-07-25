using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NodePageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string QueryNodeKey = "node";

    [ObservableProperty]
    private string? _currentState, _nodeName;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    public NodePageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryNodeKey, out var nodeName))
        {
            this.NodeName = nodeName.ToString();
        }
    }

    [ObservableProperty]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private List<TopicRowViewModel> _topics = new();

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(NodeName))
            {
                throw new InvalidOperationException("节点名称不能为空");
            }

            this.CurrentState = StateKeys.Loading;
            var node = await this.ApiService.GetNodePageInfo(this.NodeName, this.CurrentPage)
                ?? throw new InvalidOperationException("获取节点失败");
            this.CurrentPage = node.CurrentPage;
            this.MaximumPage = node.MaximumPage;
            this.Topics = node.Items
                .Select(o => TopicRowViewModel.Create(o.TopicTitle,
                    o.Avatar,
                    o.UserName,
                    o.CreatedText,
                    "",
                    o.LastReplyUserName,
                    Utilities.ParseId(o.TopicLink),
                    "",
                    o.Replies))
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
