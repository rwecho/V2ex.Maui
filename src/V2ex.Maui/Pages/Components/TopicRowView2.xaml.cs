namespace V2ex.Maui.Pages.Components;

public partial class TopicRowView2 : ContentView
{
	public TopicRowView2()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TopicProperty = BindableProperty.Create(
     nameof(Topic),
     typeof(TopicRowViewModel),
     typeof(TopicRowView));

    public TopicRowViewModel? Topic
    {
        get => (TopicRowViewModel)GetValue(TopicProperty);
        set => SetValue(TopicProperty, value);
    }
}