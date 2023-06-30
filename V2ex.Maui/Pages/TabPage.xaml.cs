using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class TabPage : ContentPage, ITransientDependency
{
	public TabPage(TabPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

    }
}