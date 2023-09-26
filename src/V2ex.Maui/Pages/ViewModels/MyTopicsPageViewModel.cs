using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class MyTopicsPageViewModel : BaseViewModel, IQueryAttributable
{
    public MyTopicsPageViewModel(NavigationManager navigationManager)
    {
        this.NavigationManager = navigationManager;
    }

    private NavigationManager NavigationManager { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LoadAll))]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private bool _isLoading;

    public bool LoadAll
    {
        get
        {
            return this.CurrentPage >= this.MaximumPage;
        }
    }

    [ObservableProperty]
    private List<TopicRowViewModel> _topics = new();

    [RelayCommand]
    public async Task GotoMyFollowingPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MyFollowingPage), true);
    }

    [RelayCommand]
    public async Task GotoMyNodesPage(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MyNodesPage), true);
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        var topicsInfo = await this.ApiService.GetFavoriteTopics() ?? throw new InvalidOperationException("获取话题失败");
        this.CurrentPage = topicsInfo.CurrentPage;
        this.MaximumPage = topicsInfo.MaximumPage;
        this.Topics = topicsInfo.Items
            .Select(o => TopicRowViewModel.Create(o.TopicTitle,
                o.Avatar,
                o.UserName,
                o.CreatedText,
                o.NodeName,
                o.LastReplyUserName,
                Utilities.ParseId(o.TopicLink),
                Utilities.ParseId(o.NodeLink),
                o.Replies))
            .ToList();
    }

    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (this.CurrentPage == this.MaximumPage || !this.Topics.Any())
        {
            return;
        }

        var nextPage = this.CurrentPage + 1;
        FavoriteTopicsInfo? favoriteTopics;

        try
        {
            this.IsLoading = true;
            favoriteTopics = await this.ApiService.GetFavoriteTopics(nextPage);
        }
        finally
        {
            this.IsLoading = false;
        }

        if (favoriteTopics == null)
        {
            throw new InvalidOperationException($"Can not get next page {nextPage} of my favorite topics.");
        }

        this.CurrentPage = favoriteTopics.CurrentPage;
        this.MaximumPage = favoriteTopics.MaximumPage;

        foreach (var item in favoriteTopics.Items)
        {
            this.Topics.Add(TopicRowViewModel.Create(item.TopicTitle,
                item.Avatar,
                item.UserName,
                item.CreatedText,
                item.NodeName,
                item.LastReplyUserName,
                Utilities.ParseId(item.TopicLink),
                Utilities.ParseId(item.NodeLink),
                item.Replies));
        }
    }
}