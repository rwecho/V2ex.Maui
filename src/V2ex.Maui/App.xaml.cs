using V2ex.Maui.Pages;
using V2ex.Maui.Services;

namespace V2ex.Maui;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        InstanceActivator.Initialize(serviceProvider);
        MainPage = serviceProvider.GetRequiredService<MainPage>();
        //MainPage = serviceProvider.GetRequiredService<AppShell.AppShell>();
    }
}