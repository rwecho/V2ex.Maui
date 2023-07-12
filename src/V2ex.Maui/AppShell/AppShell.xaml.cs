using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using V2ex.Maui.Pages;
using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.AppShell;

public partial class AppShell : Shell, ITransientDependency
{
    public AppShell(IServiceProvider serviceProvider, AppShellViewModel viewModel)
    {
        this.ServiceProvider = serviceProvider;
        InitializeComponent();
        this.BindingContext = viewModel;
        InitializeDefaultFlyoutItem();
    }

    public IServiceProvider ServiceProvider { get; }

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
            tab.Items.Add(new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
            });
        }
        MainFlyoutItem.Items.Add(tab);
#else
        foreach (var tabDefinition in tabDefinitions)
        {
            var page = this.ServiceProvider.GetRequiredService<TabPage>();
            var pageViewModel = (page.BindingContext as TabPageViewModel);
            if (pageViewModel == null)
            {
                throw new InvalidOperationException("TabPage's viewmodel can not be empty.");
            }
            pageViewModel.TabName = tabDefinition.Name;

            this.MainFlyoutItem.Items.Add(new ShellContent
            {
                Title = tabDefinition.Description,
                Content = page,
            });
        }
#endif
    }
}

public partial class AppShellViewModel : ObservableObject, ITransientDependency
{
    [RelayCommand]
    public async Task GotoSettingsPage(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage), true);
    }

    [RelayCommand]
    public async Task GotoNotificationsPage(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(nameof(NotificationsPage), true);
    }
    
    [RelayCommand]
    public async Task GotoNodesPage(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(nameof(NodesPage), true);
    }
}