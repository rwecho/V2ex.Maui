using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.Components;

public partial class HtmlView : ContentView
{
    public HtmlView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
      nameof(Text),
      typeof(string),
      typeof(HtmlView), propertyChanged: TextChanged);

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    public string? Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ResourcesService? ResourcesService { get; }

    private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
    }

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        e.Cancel = true;
    }
}