using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;
public partial class TabPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string TagPageQueryKey = "tab";

    public TabPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    [ObservableProperty]
    private string _tabName = "all";
    private ApiService ApiService { get; }

    [ObservableProperty]
    private NewsInfoViewModel? _newsInfo;

    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private bool _navBarIsVisible = true, _isReloading = false;

    [ObservableProperty]
    private Exception? exception;
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TagPageQueryKey, out var tab))
        {
            TabName = tab.ToString()!;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            this.NewsInfo = new NewsInfoViewModel(await this.ApiService.GetTabTopics(this.TabName));
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

    [RelayCommand]
    public async Task Reload(CancellationToken cancellationToken = default)
    {
        try
        {
            this.IsReloading = true;
            this.NewsInfo = new NewsInfoViewModel(await this.ApiService.GetTabTopics(this.TabName));
        }
        catch (Exception)
        {
            await Toast.Make("Reload error").Show(cancellationToken);
        }
        finally
        {
            this.IsReloading = false;
        }
    }
}

public partial class NewsInfoViewModel: ObservableObject
{
    public NewsInfoViewModel(NewsInfo newsInfo)
    {
        foreach (var item in newsInfo.Items)
        {
            this.Items.Add(InstanceActivator.Create<NewsInfoItemViewModel>(item));
        }
    }
    public ObservableCollection<NewsInfoItemViewModel> Items { get; } = new();
}

public partial class NewsInfoItemViewModel : ObservableObject, ITransientDependency
{
    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    private string _link = null!;

    [ObservableProperty]
    private string _avatar = null!;

    [ObservableProperty]
    private string _avatarLink = null!;

    [ObservableProperty]
    private string _userName = null!;

    [ObservableProperty]
    private string _userLink = null!;

    [ObservableProperty]
    private DateTime _lastReplied;

    [ObservableProperty]
    private string _nodeName = null!;

    [ObservableProperty]
    private string _nodeLink = null!;

    [ObservableProperty]
    private int _replies;

    [ObservableProperty]
    private string _id = null!, _nodeId;

    public NavigationManager NavigationManager { get; }

    public NewsInfoItemViewModel(NewsInfo.Item item, NavigationManager navigationManager)
    {
        // set all properties with item
        this.Title = item.Title;
        this.Link = item.Link;
        this.Avatar = item.Avatar;
        this.AvatarLink = item.AvatarLink;
        this.UserName = item.UserName;
        this.UserLink = item.UserLink;
        this.LastReplied = item.LastReplied;
        this.NodeName = item.NodeName;
        this.NodeLink = item.NodeLink;
        this.Replies = item.Replies;
        this.Id = item.Id;
        this.NodeId = item.NodeId;
        this.NavigationManager = navigationManager;
    }

    [RelayCommand]
    public async Task TapTitle(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            {TopicPageViewModel.QueryIdKey, this.Id },
            {"title", this.Title },
            {"link", this.Link },
            {"avatar", this.Avatar },
            {"avatarLink", this.AvatarLink },
            {"userName", this.UserName },
            {"userLink", this.UserLink },
            {"lastReplied", this.LastReplied },
            {"nodeName", this.NodeName },
            {"nodeLink", this.NodeLink },
            {"replies", this.Replies}
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

    [RelayCommand]
    public async Task TapNode(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object>
        {
            {NodePageViewModel.QueryNodeKey , this.NodeId }
        });
    }
}
