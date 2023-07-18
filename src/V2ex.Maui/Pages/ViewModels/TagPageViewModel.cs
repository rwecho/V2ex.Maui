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

public partial class TagPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string QueryTagKey = "tag";
    [ObservableProperty]
    private string? _tagName;

    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string? _currentState, _markdownHtml;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private int _currentPage = 0, _maximumPage;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private TopicViewModel? _topic;

    [ObservableProperty]
    private List<TagItemViewModel> _items = new();

    public TagPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryTagKey, out var tag))
        {
            TagName = tag.ToString()!;
        }
    }
    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(this.TagName))
            {
                throw new InvalidOperationException("Tag名称不能为空");
            }
            this.CurrentState = StateKeys.Loading;
            var tag = await this.ApiService.GetTagInfo(this.TagName, this.CurrentPage);

            this.CurrentPage = tag.CurrentPage;
            this.MaximumPage = tag.MaximumPage;
            this.Items = tag.Items.Select(o => InstanceActivator.Create<TagItemViewModel>(o)).ToList();
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception ex)
        {
            this.Exception = ex;
            this.CurrentState = StateKeys.Error;
        }
    }
}


public partial class TagItemViewModel : ObservableObject, ITransientDependency
{

    public TagItemViewModel(TagInfo.ItemInfo item, NavigationManager navigationManager)
    {
        this.Avatar = item.Avatar;
        this.UserName = item.UserName;
        this.UserLink = item.UserLink;
        this.TopicTitle = item.TopicTitle;
        this.TopicLink = item.TopicLink;
        this.Replies = item.Replies;
        this.NodeLink = item.NodeLink;
        this.NodeName = item.NodeName;
        this.NodeId = item.NodeId;
        this.Created = item.Created;
        this.CreatedText = item.CreatedText;
        this.LastReplyUserName = item.LastReplyUserName;
        this.LastReplyUserLink = item.LastReplyUserLink;
        this.Id = item.Id;
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string _nodeId, _id, _userName, _userLink, _topicTitle, _topicLink, _nodeName, _nodeLink;

    [ObservableProperty]
    private int _replies;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private string? _avatar, _createdText, _lastReplyUserName, _lastReplyUserLink;

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
            {NodePageViewModel.QueryNodeKey, NodeId }
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