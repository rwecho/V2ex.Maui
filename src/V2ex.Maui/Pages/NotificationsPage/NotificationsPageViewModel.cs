using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class NotificationsPageViewModel : BaseViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LoadAll))]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private int _total;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ObservableCollection<NotificationItemViewModel> _items = new();
    public bool LoadAll
    {
        get
        {
            return this.CurrentPage >= this.MaximumPage;
        }
    }
    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        var notificationInfo = await this.ApiService.GetNotifications() ?? throw new InvalidOperationException("获取我的消息失败");
        this.CurrentPage = notificationInfo.CurrentPage;
        this.MaximumPage = notificationInfo.MaximumPage;
        this.Total = notificationInfo.Total;

        foreach (var item in notificationInfo.Items)
        {
            this.Items.Add(InstanceActivator.Create<NotificationItemViewModel>(item));
        }
    }


    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (this.CurrentPage == this.MaximumPage || !this.Items.Any())
        {
            return;
        }

        var nextPage = this.CurrentPage + 1;
        NotificationInfo? notificationInfo;

        try
        {
            this.IsLoading = true;
            notificationInfo = await this.ApiService.GetNotifications(nextPage);
        }
        finally
        {
            this.IsLoading = false;
        }

        if (notificationInfo == null)
        {
            throw new InvalidOperationException($"Can not get next page {nextPage} of notifications.");
        }

        this.CurrentPage = notificationInfo.CurrentPage;
        this.MaximumPage = notificationInfo.MaximumPage;
        this.Total = notificationInfo.Total;

        foreach (var item in notificationInfo.Items)
        {
            this.Items.Add(InstanceActivator.Create<NotificationItemViewModel>(item));
        }
    }
}
