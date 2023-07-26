using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private int _currentPage, _maximumPage;

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
}