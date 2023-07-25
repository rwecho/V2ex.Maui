using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TagPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    public const string QueryTagKey = "tag";

    [ObservableProperty]
    private string? _tagName, _currentState;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private int _currentPage = 0, _maximumPage;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private List<TopicRowViewModel> _items = new();

    public TagPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryTagKey, out var tag))
        {
            TagName = tag.ToString()!;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(this.TagName))
            {
                throw new InvalidOperationException("Tag名称不能为空");
            }
            this.CurrentState = StateKeys.Loading;
            var tag = await this.ApiService.GetTagInfo(this.TagName, this.CurrentPage);

            this.CurrentPage = tag.CurrentPage;
            this.MaximumPage = tag.MaximumPage;
            this.Items = tag.Items.Select(o => TopicRowViewModel.Create(o.TopicTitle,
                o.Avatar,
                o.UserName,
                o.CreatedText,
                o.NodeName,
                o.LastReplyUserName,
                Utilities.ParseId(o.TopicLink),
                Utilities.ParseId(o.NodeLink),
                o.Replies)).ToList();
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception ex)
        {
            this.Exception = ex;
            this.CurrentState = StateKeys.Error;
        }
    }
}