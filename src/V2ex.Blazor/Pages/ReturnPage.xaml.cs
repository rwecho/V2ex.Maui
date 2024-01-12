
namespace V2ex.Blazor.Pages;

public partial class ReturnPage : ContentPage
{
    public ReturnPage(string targetLocation)
    {
        InitializeComponent();
        this.BindingContext = new ReturnPageViewModel(targetLocation);
        RootComponent.Parameters = new Dictionary<string, object?>
        {
            { "Url", targetLocation }
        };
    }
}