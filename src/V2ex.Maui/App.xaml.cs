using V2ex.Maui.Components;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;

namespace V2ex.Maui;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        Controls.Init();
        InitializeComponent();
        InstanceActivator.Initialize(serviceProvider);
        MainPage = serviceProvider.GetRequiredService<AppShell.AppShell>();
        //MainPage = serviceProvider.GetRequiredService<MainPage>();
    }
}
