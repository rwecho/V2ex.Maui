using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TopicPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string TopicPageQueryKey = "id";

    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private int _currentPage = 0;

    [ObservableProperty]
    private Exception? _exception;
    
    [ObservableProperty]
    private TopicInfo? _topic;
    public TopicPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }


    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TopicPageQueryKey, out var id))
        {
            Id = id.ToString()!;
        }

        if (query.TryGetValue("title", out var title))
        {
            Title = title.ToString()!;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            this.Topic = await this.ApiService.GetTopicDetail(this.Id, this.CurrentPage);
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}
