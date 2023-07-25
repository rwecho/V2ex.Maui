using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
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

    public MyTopicsPageViewModel(ApiService apiService, NavigationManager navigationManager)
    {
        this.ApiService = apiService;
        this.NavigationManager = navigationManager;
    }

    private ApiService ApiService { get; }
    private NavigationManager NavigationManager { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
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
            this.CurrentState = StateKeys.Loading;
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

            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

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
}