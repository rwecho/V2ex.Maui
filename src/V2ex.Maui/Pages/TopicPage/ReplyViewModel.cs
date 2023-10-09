using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

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