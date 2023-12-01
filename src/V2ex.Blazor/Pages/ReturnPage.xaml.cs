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
        set;
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
        get
        {
            return "";
        }
    }
}