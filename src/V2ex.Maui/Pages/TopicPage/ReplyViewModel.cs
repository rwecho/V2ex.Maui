using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class ReplyViewModel : ObservableObject
{
    public ReplyViewModel(TopicInfo.ReplyInfo reply,
        string? once,
        bool isOp,
        NavigationManager navigationManager, ICurrentUser currentUser,
        ApiService apiService,
        IStringLocalizer<MauiResource> localizer)
    {
        this.IsOp = isOp;
        this.Once = once;
        this.Id = reply.Id.Replace("r_", "");
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
        this.CurrentUser = currentUser;
        this.ApiService = apiService;
        this.Localizer = localizer;
    }

    [ObservableProperty]
    private string _id, _content, _userName, _userLink, _avatar, _replyTimeText;


    [ObservableProperty]
    private DateTime _replyTime;

    [ObservableProperty]
    private string? _badges, _once;

    [ObservableProperty]
    private int _floor, _alreadyThanked;

    [ObservableProperty]
    private bool _thanked, _isOp;


    private NavigationManager NavigationManager { get; }
    private ICurrentUser CurrentUser { get; }
    private ApiService ApiService { get; }
    private IStringLocalizer<MauiResource> Localizer { get; }

    public EventHandler<CallOutEventArgs>? CallOut;

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            { MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
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

        await this.ApiService.ThanksReplier(this.Id, this.Once);
        this.Thanked = true;
        this.AlreadyThanked += 1;

        await Toast.Make(string.Format(this.Localizer["ThanksHaveBeenSent"], this.UserName)).Show();
    }

    [RelayCommand]
    public Task UrlTap(string url)
    {
        var handler = new TapUrlHandler(url);
        switch (handler.Target)
        {
            case TapTarget.User:
                if(!string.IsNullOrEmpty(handler.UserName))
                {
                    this.CallOut?.Invoke(this, new CallOutEventArgs(this.UserName, handler.UserName, this.Floor));
                }
                break;
            case TapTarget.Image:
                break;
            case TapTarget.Link:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Task.CompletedTask;
    }
}
