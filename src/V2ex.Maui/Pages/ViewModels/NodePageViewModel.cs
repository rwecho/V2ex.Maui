using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NodePageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string QueryNameKey = "node";

    [ObservableProperty]
    private string? _currentState, _nodeName;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private List<MyNodesItemViewModel>? _nodes;

    public NodePageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryNameKey, out var nodeName))
        {
            this.NodeName = nodeName.ToString();
        }
    }

    [ObservableProperty]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private List<NodePageItemViewModel> _topics = new();

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
            this.Topics = node.Items.Select(o =>
                           InstanceActivator.Create<NodePageItemViewModel>(o)).ToList();
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class NodePageItemViewModel : ObservableObject, ITransientDependency
{
    public NodePageItemViewModel(NodePageInfo.ItemInfo item, NavigationManager navigationManager)
    {
        this.Avatar = item.Avatar;
        this.UserName = item.UserName;
        this.UserLink = item.UserLink;
        this.TopicTitle = item.TopicTitle;
        this.TopicLink = item.TopicLink;
        this.Replies = item.Replies;
        this.Created = item.Created;
        this.CreatedText = item.CreatedText;
        this.LastReplyUserName = item.LastReplyUserName;
        this.LastReplyUserLink = item.LastReplyUserLink;
        this.NavigationManager = navigationManager;
        this.Id = item.Id;
    }

    [ObservableProperty]
    private string _id, _userName, _userLink, _topicTitle, _topicLink;

    [ObservableProperty]
    private string? _avatar, _lastReplyUserName, _lastReplyUserLink, _createdText;

    [ObservableProperty]
    private int _replies;

    [ObservableProperty]
    private DateTime _created;

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task TapTitle(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            {TopicPageViewModel.QueryIdKey, this.Id }
        });
    }

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            {MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
    }
}