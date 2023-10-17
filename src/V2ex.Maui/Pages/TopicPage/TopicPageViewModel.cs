using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class TopicPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryIdKey = "id";

    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private bool _loadAll, _isLoading;

    [ObservableProperty]
    private int _currentPage = 1, _maximumPage = 0;

    [ObservableProperty]
    private TopicViewModel? _topic;

    public TopicPageViewModel(ResourcesService resourcesService, 
        NavigationManager navigationManager)
    {
        this.ResourcesService = resourcesService;
        this.NavigationManager = navigationManager;
    }

    private ResourcesService ResourcesService { get; }
    private NavigationManager NavigationManager { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryIdKey, out var id))
        {
            Id = id.ToString()!;
        }
    }

    [RelayCommand]
    public async Task TapTag(string tag, CancellationToken cancellationToken = default)
    {
        await this.NavigationManager.GoToAsync(nameof(TagPage), true, new Dictionary<string, object>
        {
            {TagPageViewModel.QueryTagKey, tag.Trim() }
        });
    }

    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (this.CurrentPage == this.MaximumPage || this.Topic == null)
        {
            return;
        }

        var nextPage = this.CurrentPage + 1;
        this.IsLoading = false;
        var topicInfo = await this.ApiService.GetTopicDetail(this.Id, nextPage);
        this.IsLoading = true;
        this.CurrentPage = topicInfo.CurrentPage;
        this.MaximumPage = topicInfo.MaximumPage;
        this.LoadAll = this.CurrentPage >= this.MaximumPage;
        this.Topic.AddNextPage(topicInfo);
    }

    [RelayCommand]
    public async Task OpenInBrowser(CancellationToken cancellationToken)
    {
        if (this.Topic == null)
        {
            return;
        }

        await Browser.Default.OpenAsync(this.Topic.Url);
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.Id))
        {
            throw new InvalidOperationException("Id 不能为空");
        }

        var topicInfo = await this.ApiService.GetTopicDetail(this.Id, this.CurrentPage);
        this.CurrentPage = topicInfo.CurrentPage;
        this.MaximumPage = topicInfo.MaximumPage;
        this.LoadAll = this.CurrentPage >= this.MaximumPage;
        this.Topic = InstanceActivator.Create<TopicViewModel>(topicInfo);
    }
}
