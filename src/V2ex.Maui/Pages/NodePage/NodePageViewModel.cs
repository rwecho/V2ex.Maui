using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class NodePageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryNodeKey = "node";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? _nodeName;

    public string Title
    {
        get
        {
            if (string.IsNullOrEmpty(this.NodeName))
            {
                return this.Localizer["NodePageTitle"];
            }
            return $"{this.Localizer[this.NodeName!]} {this.Localizer["NodePageTitle"]}";
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryNodeKey, out var nodeName))
        {
            this.NodeName = nodeName.ToString();
        }
    }

    [ObservableProperty]
    private int _currentPage = 1, _maximumPage;

    [ObservableProperty]
    private List<TopicRowViewModel> _topics = new();

    [ObservableProperty]
    private bool _loadAll;

    [ObservableProperty]
    private bool _isLoading;

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(NodeName))
        {
            throw new InvalidOperationException("节点名称不能为空");
        }

        var node = await this.ApiService.GetNodePageInfo(this.NodeName, this.CurrentPage)
            ?? throw new InvalidOperationException("获取节点失败");
        this.CurrentPage = node.CurrentPage;
        this.MaximumPage = node.MaximumPage;
        this.Topics = node.Items
            .Select(o => TopicRowViewModel.Create(o.TopicTitle,
                o.Avatar,
                o.UserName,
                o.CreatedText,
                "",
                o.LastReplyUserName,
                Utilities.ParseId(o.TopicLink),
                "",
                o.Replies))
            .ToList();
    }


    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(NodeName) || this.CurrentPage == this.MaximumPage || !this.Topics.Any())
        {
            return;
        }

        var nextPage = this.CurrentPage + 1;
        Api.NodePageInfo? node;

        try
        {
            this.IsLoading = true;
            node = await this.ApiService.GetNodePageInfo(this.NodeName, nextPage);
        }
        finally
        {
            this.IsLoading = false;
        }

        if (node == null)
        {
            throw new InvalidOperationException($"Can not get next page {nextPage} by node {this.NodeName}");
        }

        this.CurrentPage = node.CurrentPage;
        this.MaximumPage = node.MaximumPage;
        this.LoadAll = this.CurrentPage >= this.MaximumPage;


        foreach (var item in node.Items)
        {
            this.Topics.Add(TopicRowViewModel.Create(item.TopicTitle,
                item.Avatar,
                item.UserName,
                item.CreatedText,
                "",
                item.LastReplyUserName,
                Utilities.ParseId(item.TopicLink),
                "",
                item.Replies));
        }
    }
}