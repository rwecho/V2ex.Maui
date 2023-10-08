using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class MainPage : ContentPage, ITransientDependency
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        this.HtmlView.Text = AppStateManager.AppState.HtmlContainer;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
    }
}