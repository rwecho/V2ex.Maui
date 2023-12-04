namespace V2ex.Blazor.Pages;

public partial class ReturnPage : ContentPage
{
    public ReturnPage(string targetLocation)
    {
        this.TargetLocation = targetLocation;
        InitializeComponent();

        RootComponent.Parameters = new Dictionary<string, object?>
        {
            { "Url", targetLocation }
        };
    }

    public string TargetLocation
    {
        get;
        private set;
    }

    public bool HasNavigationBar
    {
        get
        {
            return DeviceInfo.Platform != DevicePlatform.WinUI;
        }
    }

    public string PageTitle
    {
        get;
        private set;
    }

    private void OnBlazorWebViewInitialized(object sender, 
        Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        //todo: how to retrive the page tile .
        //var called = await blazorWebView.TryDispatchAsync(async(services )=>
        //{
        //    var jsRuntime = services.GetRequiredService<IJSRuntime>();

        //    var pageTitle = await jsRuntime.InvokeAsync<string>("utils.getPageTitle");
        //    this.PageTitle = pageTitle;
        //});
    }
}