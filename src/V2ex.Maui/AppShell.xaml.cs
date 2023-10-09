using V2ex.Api;
using V2ex.Maui.Components;
using V2ex.Maui.Pages;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui;

public partial class AppShell : Shell, ITransientDependency
{
    public AppShell(IServiceProvider serviceProvider, AppShellViewModel viewModel,
        AppPreferencesManager appPreferencesManager)
    {
        this.ServiceProvider = serviceProvider;
        this.AppPreferencesManager = appPreferencesManager;
        InitializeComponent();
        this.BindingContext =this.ViewModel = viewModel;
        InitializeDefaultFlyoutItem();

        Application.Current!.RequestedThemeChanged += Current_RequestedThemeChanged;
        var flyoutHeader = InstanceActivator.Create<FlyoutHeader>();
        flyoutHeader.ViewModel.OnNavigationChanged += FlyoutHeader_OnNavigationChanged;
        this.FlyoutHeader = flyoutHeader;

        this.FlyoutFooter = InstanceActivator.Create<FlyoutFooter>();
    }

    private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        //todo: how to change the theme of the app?
    }

    private void FlyoutHeader_OnNavigationChanged(object? sender, EventArgs e)
    {
        this.ViewModel.FlyoutIsPresented = false;
    }
    private IServiceProvider ServiceProvider { get; }
    public AppPreferencesManager AppPreferencesManager { get; }
    private AppShellViewModel ViewModel { get; }

    protected override bool OnBackButtonPressed()
    {
        var result = base.OnBackButtonPressed();

        return result;
    }

    private void InitializeDefaultFlyoutItem()
    {
        var tabDefinitions = TabDefinition.GetTabDefinitions();

#if ANDROID || IOS
        var tab = new Tab
        {
            Title = "TabPage",
        };

        foreach (var tabDefinition in tabDefinitions)
        {
            var page = this.ServiceProvider.GetRequiredService<TabPage>();
            var pageViewModel = (page.BindingContext as TabPageViewModel);
            if (pageViewModel == null)
            {
                throw new InvalidOperationException("TabPage's view model can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;

            var shellContent = new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
            };
            tab.Items.Add(shellContent);

            if (this.AppPreferencesManager.Current?.LatestTabName == tabDefinition.Name)
            {
                // issue: the default opened tab is blank.
                tab.CurrentItem = shellContent;
            }
        }

        MainFlyoutItem.Items.Add(tab);
#else
        foreach (var tabDefinition in tabDefinitions)
        {
            var page = this.ServiceProvider.GetRequiredService<TabPage>();
            var pageViewModel = (page.BindingContext as TabPageViewModel);
            if (pageViewModel == null)
            {
                throw new InvalidOperationException("TabPage's view model can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;

          
            var shellContent = new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
            };
            this.MainFlyoutItem.Items.Add(shellContent);

            if (this.AppPreferencesManager.Current?.LatestTabName == tabDefinition.Name)
            {
                this.MainFlyoutItem.CurrentItem = shellContent;
            }
        }
#endif
    }
}
