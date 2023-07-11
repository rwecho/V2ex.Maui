using Microsoft.Extensions.DependencyInjection;
using V2ex.Api;
using V2ex.Maui.Pages;
using V2ex.Maui.Pages.ViewModels;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Shell;

public partial class AppShell : Microsoft.Maui.Controls.Shell, ITransientDependency
{
    public AppShell(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
        InitializeComponent();
        InitialzieDefaultFlyoutItem();
    }

    public IServiceProvider ServiceProvider { get; }
    protected override bool OnBackButtonPressed()
    {
        var result = base.OnBackButtonPressed();

        return result;
    }

    private void InitialzieDefaultFlyoutItem()
    {
        var tabDefinitions = TabDefinition.GetTabDefinitions();

#if ANDROID || IOS
        var flyoutItem = new FlyoutItem
        {
            Title = "MainPage",
        };
        AppShell.SetFlyoutBehavior(flyoutItem, FlyoutBehavior.Flyout);
        AppShell.SetFlyoutItemIsVisible(flyoutItem, false);
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
                throw new InvalidOperationException("TabPage's viewmodel can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;
            tab.Items.Add(new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
                Route = $"tab/{tabDefinition.Name}"
            });
        }

        flyoutItem.Items.Add(tab);
        this.Items.Add(flyoutItem);
#else

        var item = new FlyoutItem
        {
            FlyoutItemIsVisible = false,
        };
        this.Items.Add(item);
        foreach (var tabDefinition in tabDefinitions)
        {
            var page = this.ServiceProvider.GetRequiredService<TabPage>();
            var pageViewModel = (page.BindingContext as TabPageViewModel);
            if (pageViewModel == null)
            {
                throw new InvalidOperationException("TabPage's viewmodel can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;

         
            item.Items.Add(new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
            });
        }
#endif
    }
}