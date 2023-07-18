using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class MyTopicsPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private List<MyNodesItemViewModel>? _nodes;

    public MyTopicsPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [ObservableProperty]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private List<MyTopicsItemViewModel> _topics = new();

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            var topicsInfo = await this.ApiService.GetFavoriteTopics() ?? throw new InvalidOperationException("获取话题失败");
            this.CurrentPage = topicsInfo.CurrentPage;
            this.MaximumPage = topicsInfo.MaximumPage;
            this.Topics = topicsInfo.Items.Select(o=> 
                InstanceActivator.Create<MyTopicsItemViewModel>(o)).ToList();

            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class MyTopicsItemViewModel: ObservableObject
{
    public MyTopicsItemViewModel(FavoriteTopicsInfo.ItemInfo item, NavigationManager navigationManager)
    {
        this.Avatar = item.Avatar;
        this.UserName= item.UserName;
        this.UserLink= item.UserLink;
        this.TopicTitle= item.TopicTitle;
        this.TopicLink= item.TopicLink;
        this.Replies= item.Replies;
        this.NodeName= item.NodeName;
        this.NodeLink= item.NodeLink;
        this.NodeId = item.NodeId;
        this.Created= item.Created;
        this.CreatedText= item.CreatedText;
        this.LastReplyUserName= item.LastReplyUserName;
        this.LastReplyUserLink = item.LastReplyUserLink;
        this.NavigationManager = navigationManager;
        this.Id = item.Id;
    }


    [ObservableProperty]
    private string _nodeId, _id, _userName, _userLink, _topicTitle, _topicLink, _nodeName, _nodeLink, _createdText;

    [ObservableProperty]
    private string? _avatar, _lastReplyUserName, _lastReplyUserLink;

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
    public async Task TapNode(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object>
        {
            {NodePageViewModel.QueryNameKey, this.NodeId }
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
