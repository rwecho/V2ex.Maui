using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class NodePageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryNodeKey = "node";

    [ObservableProperty]
    private string? _nodeName;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryNodeKey, out var nodeName))
        {
            this.NodeName = nodeName.ToString();
        }
    }

    [ObservableProperty]
    private int _currentPage, _maximumPage;

    [ObservableProperty]
    private List<TopicRowViewModel> _topics = new();

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
}