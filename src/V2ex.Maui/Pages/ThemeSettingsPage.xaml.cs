using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class ThemeSettingsPage : ContentPage, ITransientDependency
{
	public ThemeSettingsPage(ThemeSettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.ViewModel = viewModel;
	}

    public ThemeSettingsViewModel ViewModel { get; }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel.Load());
    }
}