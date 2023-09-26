using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TagPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryTagKey = "tag";

    [ObservableProperty]
    private string? _tagName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LoadAll))]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private bool _isLoading;

    public bool LoadAll
    {
        get
        {
            return this.CurrentPage >= this.MaximumPage;
        }
    }

    [ObservableProperty]
    private List<TopicRowViewModel> _items = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryTagKey, out var tag))
        {
            TagName = tag.ToString()!;
        }
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.TagName))
        {
            throw new InvalidOperationException("Tag名称不能为空");
        }
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
    }

    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.TagName) || this.CurrentPage == this.MaximumPage || !this.Items.Any())
        {
            return;
        }

        var nextPage = this.CurrentPage + 1;
        TagInfo? tagInfo;

        try
        {
            this.IsLoading = true;
            tagInfo = await this.ApiService.GetTagInfo(this.TagName, nextPage);
        }
        finally
        {
            this.IsLoading = false;
        }

        if (tagInfo == null)
        {
            throw new InvalidOperationException($"Can not get next page {nextPage} of my favorite topics.");
        }

        this.CurrentPage = tagInfo.CurrentPage;
        this.MaximumPage = tagInfo.MaximumPage;

        foreach (var item in tagInfo.Items)
        {
            this.Items.Add(TopicRowViewModel.Create(item.TopicTitle,
                item.Avatar,
                item.UserName,
                item.CreatedText,
                item.NodeName,
                item.LastReplyUserName,
                Utilities.ParseId(item.TopicLink),
                Utilities.ParseId(item.NodeLink),
                item.Replies));
        }
    }
}