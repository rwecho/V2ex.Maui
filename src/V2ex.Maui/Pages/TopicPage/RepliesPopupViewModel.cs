using CommunityToolkit.Mvvm.ComponentModel;

namespace V2ex.Maui.Pages;

public class RepliesPopupViewModel: ObservableObject
{
    public RepliesPopupViewModel(IReadOnlyList<ReplyViewModel> replies)
    {
        this.Replies = replies;
        this.Count = replies.Count;

        this.UserName = replies.FirstOrDefault()?.UserName;
    }
    public IReadOnlyList<ReplyViewModel> Replies { get;  }

    public int Count { get; }

    public string? UserName { get; }
}