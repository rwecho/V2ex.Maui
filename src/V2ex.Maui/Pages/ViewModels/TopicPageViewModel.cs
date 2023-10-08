﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

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

    public TopicPageViewModel(ResourcesService resourcesService, NavigationManager navigationManager)
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

public partial class SupplementViewModel : ObservableObject
{
    public SupplementViewModel(int index, TopicInfo.SupplementInfo item)
    {
        this.Index = index;
        this.Content = item.Content;
        this.Created = item.Created;
        this.CreatedText = item.CreatedText;
    }

    [ObservableProperty]
    private int _index;
    [ObservableProperty]
    private string? _content, _createdText;
    [ObservableProperty]
    private DateTime _created;
}

public partial class ReplyViewModel : ObservableObject
{
    public ReplyViewModel(TopicInfo.ReplyInfo reply, NavigationManager navigationManager)
    {
        this.Content = reply.Content;
        this.UserName = reply.UserName;
        this.UserLink = reply.UserLink;
        this.Avatar = reply.Avatar;
        this.ReplyTime = reply.ReplyTime;
        this.ReplyTimeText = reply.ReplyTimeText;
        this.Badges = reply.Badges;
        this.Floor = reply.Floor;
        this.Thanked = reply.Thanked != null;
        this.AlreadyThanked = reply.AlreadyThanked == null ? 0 : int.Parse(reply.AlreadyThanked);
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string _content, _userName, _userLink, _avatar, _replyTimeText;


    [ObservableProperty]
    private DateTime _replyTime;

    [ObservableProperty]
    private string? _badges;

    [ObservableProperty]
    private int _floor, _alreadyThanked;

    [ObservableProperty]
    private bool _thanked;


    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            { MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
    }

    [RelayCommand]
    public Task TapThank(CancellationToken cancellationToken)
    {
        //todo: confirm & cancel thank status to reply.
        return Task.CompletedTask;
    }
}