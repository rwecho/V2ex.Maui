using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Localization;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class TopicViewModel : ObservableObject
{
    public TopicViewModel(TopicInfo topic, NavigationManager navigationManager,
        ICurrentUser currentUser,
        ApiService apiService,
        IStringLocalizer<MauiResource> localizer)
    {
        this.Once = topic.Once;
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
        var (views, likes, thanks) = ParseTopicStats(topic.TopicStats);
        this.Views = views;
        this.Likes = likes;
        this.Thanks = thanks;
        this.Tags = topic.Tags;
        this.CurrentPage = topic.CurrentPage;
        this.MaximumPage = topic.MaximumPage;
        this.Supplements = topic.Supplements
            .Select((o, index) => new SupplementViewModel(index, o))
            .ToList();
        this.Replies = new ObservableCollection<ReplyViewModel>(
            topic.Replies.Select(CreateReplyViewModel));
        this.NavigationManager = navigationManager;
        this.CurrentUser = currentUser;
        this.ApiService = apiService;
        this.Localizer = localizer;
        this.Url = UrlUtilities.CompleteUrl(topic.Url);
        this.Id = ParseTopicId(topic.Url);
        this.Thanked = topic.Thanked == "感谢已发送";
        this.Liked = topic.Liked == "取消收藏";
    }

    private static string ParseTopicId(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new InvalidOperationException("url is null");
        }
        var idRegex = new Regex("/t/(\\d+)");
        var match = idRegex.Match(url);
        if (!match.Success)
        {
            throw new InvalidOperationException("url is invalid");
        }
        return match.Groups[1].Value;
    }

    private static (int Views, int Likes, int Thanks) ParseTopicStats(string? topicStats)
    {
        if (string.IsNullOrEmpty(topicStats))
        {
            return (0, 0, 0);
        }

        var viewsRegex = new Regex("(\\d+)\\s+views");
        var likesRegex = new Regex("(\\d+)\\s+likes");
        var thanksRegex = new Regex("(\\d+)\\s+人");

        var viewsMatch = viewsRegex.Match(topicStats);
        int views = 0, likes = 0, thanks = 0;
        if (viewsMatch.Success)
        {
            views = int.Parse(viewsMatch.Groups[1].Value);
        }

        var likesMatch = likesRegex.Match(topicStats);
        if (likesMatch.Success)
        {
            likes = int.Parse(likesMatch.Groups[1].Value);
        }

        var thanksMatch = thanksRegex.Match(topicStats);
        if (thanksMatch.Success)
        {
            thanks = int.Parse(thanksMatch.Groups[1].Value);
        }

        return (views, likes, thanks);
    }

    internal void AddNextPage(TopicInfo topic)
    {
        this.CurrentPage = topic.CurrentPage;
        this.MaximumPage = topic.MaximumPage;
        this.ReplyStats = topic.ReplyStats;

        foreach (var item in topic.Replies)
        {
            this.Replies.Add(CreateReplyViewModel(item));
        }
    }

    private ReplyViewModel CreateReplyViewModel(TopicInfo.ReplyInfo item)
    {
        var isOp = this.UserName == item.UserName;
        var replyViewModel = InstanceActivator.Create<ReplyViewModel>(item, this.Once ?? "",
            isOp);
        replyViewModel.CallOut += this.CallOut;
        return replyViewModel;
    }

    private void CallOut(object? sender, CallOutEventArgs e)
    {
        var replies = this.Replies.Where(o => o.UserName == e.Target && o.Floor < e.Floor)
            .ToArray();
        WeakReferenceMessenger.Default.Send(new CallOutRepliesMessage(replies));
    }

    [ObservableProperty]
    private string _id, _title, _nodeId, _userName, _url, _userLink,
        _avatar, _createdText, _nodeName, _nodeLink;

    [ObservableProperty]
    private string? _content, _once, _replyStats;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private List<string> _tags;

    [ObservableProperty]
    private List<SupplementViewModel> _supplements;

    [ObservableProperty]
    private int _currentPage, _maximumPage, _thanks, _views, _likes;
    [ObservableProperty]
    private bool _liked, _thanked, _isInputting; // the IsInputting represents the ReplyPopup is showing and accepting input

    [ObservableProperty]
    private ObservableCollection<ReplyViewModel> _replies;

    [ObservableProperty]
    private string? _inputText = "hello world";

    private NavigationManager NavigationManager { get; }
    private ICurrentUser CurrentUser { get; }
    private ApiService ApiService { get; }
    private IStringLocalizer<MauiResource> Localizer { get; }

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
    public async Task TapLike(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.Id) ||
         string.IsNullOrEmpty(this.Once) ||
         !this.CurrentUser.IsAuthorized() )
        {
            return;
        }

        if (this.Liked)
        {
            await this.ApiService.UnfavoriteTopic(this.Id, this.Once);
            this.Liked = false;
            this.Likes = Math.Max(0, this.Likes - 1);
            await Toast.Make(string.Format(this.Localizer["TopicUnfavoriteSuccess"])).Show();
        }
        else
        {
            await this.ApiService.FavoriteTopic(this.Id, this.Once);
            this.Liked = true;
            this.Likes += 1;
            await Toast.Make(string.Format(this.Localizer["TopicFavoriteSuccess"])).Show();
        }
    }

    [RelayCommand]
    public async Task TapThank(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.Id) ||
            string.IsNullOrEmpty(this.Once) ||
            !this.CurrentUser.IsAuthorized() ||
            this.Thanked)
        {
            return;
        }

        var result = await this.ApiService.ThankCreator(this.Id, this.Once);
        if (result?.Success == true)
        {
            this.Thanked = true;
            this.Thanks += 1;

            await Toast.Make(string.Format(this.Localizer["TopicThankSuccess"])).Show();
        }
        else if(result!= null && result.Message !=null)
        {
            await Toast.Make(result.Message).Show();
        }
    }

    [RelayCommand]
    public async Task SubmitReply(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.InputText) || string.IsNullOrEmpty(this.Once))
        {
            return;
        }

        await this.ApiService.ReplyTopic(this.Id, this.InputText, this.Once);
        this.InputText = string.Empty;
    }
}
