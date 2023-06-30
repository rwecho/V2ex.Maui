using V2ex.Maui.Pages;
using V2ex.Maui.Pages.ViewModels;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Shell;

public partial class AppShell : Microsoft.Maui.Controls.Shell, ITransientDependency
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var tabDefinitions = TabDefinition.GetTabDefinitions();

        foreach (var tabDefinition in tabDefinitions)
        {
            var page = serviceProvider.GetRequiredService<TabPage>();
            var pageViewModel = (page.BindingContext as TabPageViewModel);
            if (pageViewModel == null)
            {
                throw new InvalidOperationException("TabPage's viewmodel can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;
            this.MainPageTab.Items.Add(new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
                Route = $"tab/{tabDefinition.Name}"
            });
        }
    }
}