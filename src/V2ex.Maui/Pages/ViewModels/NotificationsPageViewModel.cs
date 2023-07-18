using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NotificationsPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState;
    [ObservableProperty]
    private Exception? _exception;
    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private NotificationViewModel? _notification;

    public NotificationsPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            var notificationInfo = await this.ApiService.GetNotifications() ?? throw new InvalidOperationException("获取我的消息失败");
            this.Notification = new (notificationInfo);
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task LoadMore(CancellationToken cancellationToken = default)
    {
        // not triggered on Windows, https://github.com/dotnet/maui/issues/9935 fixed in the 8.0.0-preview.5.8529
        try
        {
            this.CurrentState = StateKeys.Loading;
            if(this.Notification == null)
            {
                throw new InvalidOperationException("未加载数据");
            }

            if(this.Notification.CurrentPage >= this.Notification.MaximumPage)
            {
                return;
            }

            var page = this.Notification.CurrentPage + 1;
            var notificationInfo = await this.ApiService.GetNotifications(page) ?? throw new InvalidOperationException("获取我的消息失败");

            foreach (var item in notificationInfo.Items)
            {
                this.Notification.Items.Add(InstanceActivator.Create<NotificationItemViewModel>(item));
            }
            this.Notification.CurrentPage = page;
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public partial class NotificationViewModel : ObservableObject
{
    public NotificationViewModel(NotificationInfo notificationInfo)
    {
        this.Total = notificationInfo.Total;
        this.CurrentPage = notificationInfo.CurrentPage;
        this.MaximumPage= notificationInfo.MaximumPage;
        this.Items = notificationInfo.Items
            .Select(o=> InstanceActivator.Create<NotificationItemViewModel>(o))
            .ToList();
    }

    [ObservableProperty]
    private int _total, _currentPage, _maximumPage;
    [ObservableProperty]
    public List<NotificationItemViewModel> _items;
}

public partial class NotificationItemViewModel: ObservableObject
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
        this.Id = item.Id;
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
            {MemberPageViewModel.UserNameKey, this.UserName}
        });
    }

    [RelayCommand()]
    public async Task GotoTopic(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            {TopicPageViewModel.TopicPageQueryIdKey, this.Id}
        });
    }
}
