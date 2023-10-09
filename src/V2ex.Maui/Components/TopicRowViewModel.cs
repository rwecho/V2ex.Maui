using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Components;

public partial class TopicRowViewModel : ObservableObject, ITransientDependency
{
    [ObservableProperty]
    private string? _title, _avatar, _userName, _createdText, _nodeName, _lastReplyBy, _topicId, _nodeId;

    [ObservableProperty]
    private int _replies;

    public TopicRowViewModel(NavigationManager navigationManager)
    {
        this.NavigationManager = navigationManager;
    }

    private NavigationManager NavigationManager { get; }

    [RelayCommand]
    public async Task TapUser(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.UserName))
        {
            return;
        }
        await this.NavigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
        {
            { MemberPageViewModel.QueryUserNameKey, this.UserName }
        });
    }

    [RelayCommand]
    public async Task TapTitle(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.TopicId))
        {
            return;
        }
        await this.NavigationManager.GoToAsync(nameof(TopicPage), true, new Dictionary<string, object>
        {
            { TopicPageViewModel.QueryIdKey, this.TopicId }
        });
    }

    [RelayCommand]
    public async Task TapNode(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.NodeId))
        {
            return;
        }
        await this.NavigationManager.GoToAsync(nameof(NodePage), true, new Dictionary<string, object>
        {
            { NodePageViewModel.QueryNodeKey, this.NodeId }
        });
    }

    public static TopicRowViewModel Create(
        string? title, string? avatar,
        string? userName, string? createdText,
        string? nodeName, string? lastReplyBy,
        string topicId, string? nodeId,
        int replies)
    {
        var instance = InstanceActivator.Create<TopicRowViewModel>();
        instance.Title = title;
        instance.Avatar = avatar;
        instance.UserName = userName;
        instance.CreatedText = createdText;
        instance.NodeName = nodeName;
        instance.LastReplyBy = lastReplyBy;
        instance.TopicId = topicId;
        instance.NodeId = nodeId;
        instance.Replies = replies;

        return instance;
    }
}
