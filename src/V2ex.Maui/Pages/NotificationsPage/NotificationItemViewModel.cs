using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class NotificationItemViewModel : ObservableObject
{
    public NotificationItemViewModel(NotificationInfo.NotificationItemInfo item,
        NavigationManager navigationManager)
    {
        this.UserName = item.UserName;
        this.UserLink = item.UserLink;
        this.Avatar = item.Avatar;
        this.TopicLink = item.TopicLink;
        this.TopicTitle = item.TopicTitle;
        this.PreTitle = item.PreTitle;
        this.PostTitle = item.PostTitle;
        this.Created = item.Created;
        this.Payload = item.Payload;
        this.Id = Utilities.ParseId(item.TopicLink);
        this.NavigationManager = navigationManager;
    }

    [ObservableProperty]
    private string? _preTitle, _postTitle, _created, _payload;

    [ObservableProperty]
    private string _id, _userName, _userLink, _avatar, _topicLink, _topicTitle;

    private NavigationManager NavigationManager { get; }

    [RelayCommand()]
    public async Task GotoUser(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            {MemberPageViewModel.QueryUserNameKey, this.UserName}
        });
    }

    [RelayCommand()]
    public async Task GotoTopic(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            {TopicPageViewModel.QueryIdKey, this.Id}
        });
    }
}