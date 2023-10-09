using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class MainPage : ContentPage, ITransientDependency
{
    public MainPage()
    {
        InitializeComponent();
    }
}