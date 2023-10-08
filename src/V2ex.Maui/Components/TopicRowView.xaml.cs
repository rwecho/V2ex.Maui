namespace V2ex.Maui.Components;

public partial class TopicRowView : ContentView
{
    public TopicRowView()
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
