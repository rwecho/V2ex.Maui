using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Components;

namespace V2ex.Maui.Pages;

public partial class NewsInfoViewModel : ObservableObject
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