using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class SearchItemViewModel : ObservableObject, ITransientDependency
{
    public SearchItemViewModel(NavigationManager navigationManager)
    {
        this.NavigationManager = navigationManager;
    }
    [ObservableProperty]
    private string _content = null!, _title = null!, _creator = null!;

    [ObservableProperty]
    private DateTime _created;

    [ObservableProperty]
    private long _replies;

    [ObservableProperty]
    private int _id;

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task TapTopic(CancellationToken cancellationToken)
    {
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            { TopicPageViewModel.QueryIdKey, this.Id.ToString() }
        });
    }
}