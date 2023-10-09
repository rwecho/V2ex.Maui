using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;

namespace V2ex.Maui.Pages;

public partial class SupplementViewModel : ObservableObject
{
    public SupplementViewModel(int index, TopicInfo.SupplementInfo item)
    {
        this.Index = index+1;
        this.Content = item.Content;
        this.Created = item.Created;
        this.CreatedText = item.CreatedText;
    }

    [ObservableProperty]
    private int _index;
    [ObservableProperty]
    private string? _content, _createdText;
    [ObservableProperty]
    private DateTime _created;
}
