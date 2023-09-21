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

    public event EventHandler<string>? UrlTapped;

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
        var url = e.Url;
        this.UrlTapped?.Invoke(this, url);
        e.Cancel = true;
    }

    private async void WebView_Loaded(object sender, EventArgs e)
    {
        try
        {

            if (int.TryParse(await this.WebView.EvaluateJavaScriptAsync($"document.body.scrollHeight"), out var clientHeight) && clientHeight > 0)
            {
                this.WebView.HeightRequest = clientHeight;

            }
        }
        catch (Exception exception)
        {

        }
    }

}