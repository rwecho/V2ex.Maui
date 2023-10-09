using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class TopicViewModel : ObservableObject
{
    public TopicViewModel(TopicInfo topic, NavigationManager navigationManager)
    {
        this.Title = topic.Title;
        this.Content = topic.Content;
        this.UserName = topic.UserName;
        this.UserLink = topic.UserLink;
        this.Avatar = topic.Avatar;
        this.Created = topic.Created;
        this.CreatedText = topic.CreatedText;
        this.ReplyStats = topic.ReplyStats;
        this.NodeName = topic.NodeName;
        this.NodeLink = topic.NodeLink;
        this.NodeId = topic.NodeId;
        ParseTopicStats(topic.TopicStats);
        this.Tags = topic.Tags;
        this.CurrentPage = topic.CurrentPage;
        this.MaximumPage = topic.MaximumPage;
        this.Supplements = topic.Supplements
            .Select((o, index) => new SupplementViewModel(index, o))
            .ToList();
        this.Replies = new ObservableCollection<ReplyViewModel>(
            topic.Replies.Select(x => InstanceActivator.Create<ReplyViewModel>(x)));
        this.NavigationManager = navigationManager;

        this.Url = UrlUtilities.CompleteUrl(topic.Url);
        this.Thanked = topic.Thanked == "感谢已发送";
        this.Liked = topic.Liked == "取消收藏";
    }

    private void ParseTopicStats(string? topicStats)
    {
        if (string.IsNullOrEmpty(topicStats))
        {
            return;
        }

        var viewsRegex = new Regex("(\\d+)\\s+views");
        var likesRegex = new Regex("(\\d+)\\s+likes");
        var thanksRegex = new Regex("(\\d+)\\s+人");

        var viewsMatch = viewsRegex.Match(topicStats);
        if (viewsMatch.Success)
        {
            this.Views = int.Parse(viewsMatch.Groups[1].Value);
        }

        var likesMatch = likesRegex.Match(topicStats);
        if (likesMatch.Success)
        {
            this.Likes = int.Parse(likesMatch.Groups[1].Value);
        }

        var thanksMatch = thanksRegex.Match(topicStats);
        if (thanksMatch.Success)
        {
            this.Thanks = int.Parse(thanksMatch.Groups[1].Value);
        }
    }

    internal void AddNextPage(TopicInfo topic)
    {
        this.CurrentPage = topic.CurrentPage;
        this.MaximumPage = topic.MaximumPage;
        this.ReplyStats = topic.ReplyStats;

        foreach (var item in topic.Replies)
        {
            var replyViewModel = InstanceActivator.Create<ReplyViewModel>(item);
            this.Replies.Add(replyViewModel);
        }
    }

    [ObservableProperty]
    private string _title, _nodeId;

    [ObservableProperty]
    private string? _content;

    [ObservableProperty]
    private string _userName, _url;

    [ObservableProperty]
    private string _userLink;

    [ObservableProperty]
    private string _avatar;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private string _createdText;

    [ObservableProperty]
    private string? _replyStats;

    [ObservableProperty]
    private string _nodeName;

    [ObservableProperty]
    private string _nodeLink;

    [ObservableProperty]
    private List<string> _tags;

    [ObservableProperty]
    private List<SupplementViewModel> _supplements;

    [ObservableProperty]
    private int _currentPage, _maximumPage, _thanks, _views, _likes;
    [ObservableProperty]
    private bool _liked, _thanked;

    [ObservableProperty]
    private ObservableCollection<ReplyViewModel> _replies;

    private NavigationManager NavigationManager { get; }


    [RelayCommand]
    public async Task TapNode(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object>
        {
            { NodePageViewModel.QueryNodeKey, this.NodeId }
        });
    }

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            { MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
    }

    [RelayCommand]
    public Task TapLike(CancellationToken cancellationToken)
    {
        // todo: Implement like action.
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task TapThank(CancellationToken cancellationToken)
    {
        // todo: implement thank action.
        return Task.CompletedTask;
    }
}
