using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using V2ex.Api;
using V2ex.Maui.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages;

public partial class NewsInfoViewModel : ObservableObject
{
    public NewsInfoViewModel(NewsInfo newsInfo, UserManager userManager)
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

        // When we enter the tab page, check if we are is logged in.
        if (newsInfo.CurrentUser == null)
        {
            userManager.Logout();
        }
        else
        {
            userManager.Login(newsInfo.CurrentUser);
        }
    }

    public ObservableCollection<TopicRowViewModel> Items { get; } = new();
}