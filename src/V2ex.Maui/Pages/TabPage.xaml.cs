using V2ex.Maui.Pages.ViewModels;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TabPage : ContentPage, ITransientDependency
{
    private TabPageViewModel ViewModel { get; }
    private AppPreferencesManager AppPreferencesManager { get; }

    public TabPage(TabPageViewModel viewModel, AppPreferencesManager appPreferencesManager)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
        this.AppPreferencesManager = appPreferencesManager;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }

        var appPreferences = this.AppPreferencesManager.Current ?? new();
        appPreferences.LatestTabName = this.ViewModel.TabName;
        this.AppPreferencesManager.Set(appPreferences);
        Task.Run(() => this.ViewModel!.Load());
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // todo: how to hide/show nav bar when scrolling?
    }
}