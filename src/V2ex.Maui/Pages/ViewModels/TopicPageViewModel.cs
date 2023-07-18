using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TopicPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string QueryIdKey = "id";

    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string? _currentState, _markdownHtml;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private int _currentPage = 0;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private TopicViewModel? _topic;

    public TopicPageViewModel(ApiService apiService, ResourcesService resourcesService)
    {
        this.ApiService = apiService;
        this.ResourcesService = resourcesService;
    }

    private ApiService ApiService { get; }
    public ResourcesService ResourcesService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryIdKey, out var id))
        {
            Id = id.ToString()!;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            this.MarkdownHtml = await this.ResourcesService.GetMarkdownContainer();
            this.Topic = new TopicViewModel(await this.ApiService.GetTopicDetail(this.Id, this.CurrentPage), this.MarkdownHtml);
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class TopicViewModel : ObservableObject
{
    public TopicViewModel(TopicInfo topic, string markdownHtml)
    {
        this.Title = topic.Title;
        this.Content = markdownHtml.Replace("@markdown", topic.Content);
        this.UserName = topic.UserName;
        this.UserLink = topic.UserLink;
        this.Avatar = topic.Avatar;
        this.Created = topic.Created;
        this.CreatedText = topic.CreatedText;
        this.TopicStats = topic.TopicStats;
        this.NodeName = topic.NodeName;
        this.NodeLink = topic.NodeLink;
        this.ReplyStats = topic.ReplyStats;
        this.Tags = topic.Tags;
        this.CurrentPage = topic.CurrentPage;
        this.MaximumPage = topic.MaximumPage;
        this.Replies = new ObservableCollection<ReplyViewModel>(
            topic.Replies.Select(x => new ReplyViewModel(x)));
    }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _content;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _userLink;

    [ObservableProperty]
    private string _avatar;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private string _createdText;

    [ObservableProperty]
    private string? _topicStats;

    [ObservableProperty]
    private string _nodeName;

    [ObservableProperty]
    private string _nodeLink;

    [ObservableProperty]
    private string? _replyStats;

    [ObservableProperty]
    private List<string> _tags;

    [ObservableProperty]
    private int _currentPage;

    [ObservableProperty]
    private int _maximumPage;

    [ObservableProperty]
    private ObservableCollection<ReplyViewModel> _replies;
}

public partial class ReplyViewModel : ObservableObject
{
    public ReplyViewModel(TopicInfo.ReplyInfo reply)
    {
        this.Content = reply.Content;
        this.UserName = reply.UserName;
        this.UserLink = reply.UserLink;
        this.Avatar = reply.Avatar;
        this.ReplyTime = reply.ReplyTime;
        this.ReplyTimeText = reply.ReplyTimeText;
        this.Badges = reply.Badges;
        this.Floor = reply.Floor;
        this.AlreadyThanked = reply.AlreadyThanked;
    }

    [ObservableProperty]
    private string _content;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _userLink;

    [ObservableProperty]
    private string _avatar;

    [ObservableProperty]
    private DateTime _replyTime;

    [ObservableProperty]
    private string _replyTimeText;

    [ObservableProperty]
    private string? _badges;

    [ObservableProperty]
    private int _floor;

    [ObservableProperty]
    private string? _alreadyThanked;

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
    }
}