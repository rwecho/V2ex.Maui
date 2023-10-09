using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Components;

namespace V2ex.Maui.Pages;

public partial class TabPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string TagPageQueryKey = "tab";

    [ObservableProperty]
    private string _tabName = "all";

    [ObservableProperty]
    private NewsInfoViewModel? _newsInfo;

    [ObservableProperty]
    private bool _navBarIsVisible = true, _isReloading = false;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(TagPageQueryKey, out var tab))
        {
            TabName = tab.ToString()!;
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

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        this.NewsInfo = new NewsInfoViewModel(await this.ApiService.GetTabTopics(this.TabName));
    }
}
