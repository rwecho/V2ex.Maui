using CommunityToolkit.Mvvm.ComponentModel;

namespace V2ex.Maui.Pages.Components;

public partial class TopicView : ContentView
{
	public TopicView()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TopicProperty = BindableProperty.Create(
		nameof(Topic),
		typeof(TopicViewModel),
		typeof(TopicView),
		null);

	public TopicViewModel? Topic
	{
        get => (TopicViewModel)GetValue(TopicProperty);
        set => SetValue(TopicProperty, value);
    }
}

public partial class TopicViewModel : ObservableObject
{
	[ObservableProperty]
	private string? _title, _avatar, _userName, _createdText, _nodeName, _lastReplyBy;

	[ObservableProperty]
	private int _replies;
}