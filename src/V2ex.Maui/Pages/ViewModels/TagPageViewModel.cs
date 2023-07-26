using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class TagPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryTagKey = "tag";

    [ObservableProperty]
    private string? _tagName;

    [ObservableProperty]
    private int _currentPage = 0, _maximumPage;

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
}