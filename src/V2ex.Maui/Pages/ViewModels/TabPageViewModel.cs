using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;
public partial class TabPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string TagPageQueryKey = "tab";

    public TabPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    [ObservableProperty]
    private string _tabName = "all";
    private ApiService ApiService { get; }

    [ObservableProperty]
    private NewsInfoViewModel? _newsInfo;

    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private bool _navBarIsVisible = true, _isReloading = false;

    [ObservableProperty]
    private Exception? exception;
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TagPageQueryKey, out var tab))
        {
            TabName = tab.ToString()!;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            this.NewsInfo = new NewsInfoViewModel(await this.ApiService.GetTabTopics(this.TabName));
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

    [RelayCommand]
    public async Task Reload(CancellationToken cancellationToken = default)
    {
        try
        {
            this.IsReloading = true;
            this.NewsInfo = new NewsInfoViewModel(await this.ApiService.GetTabTopics(this.TabName));
        }
        catch (Exception)
        {
            await Toast.Make("Reload error").Show(cancellationToken);
        }
        finally
        {
            this.IsReloading = false;
        }
    }
}

public partial class NewsInfoViewModel: ObservableObject
{
    public NewsInfoViewModel(NewsInfo newsInfo)
    {
        foreach (var item in newsInfo.Items)
        {
            this.Items.Add(TopicRowViewModel.Create(item.Title,
                item.Avatar,
                item.UserName,
                item.LastRepliedText ?? "",
                item.NodeName,
                item.LastRepliedBy,
                item.Id,
                item.NodeId,
                item.Replies));
        }
    }
    public ObservableCollection<TopicRowViewModel> Items { get; } = new();
}
