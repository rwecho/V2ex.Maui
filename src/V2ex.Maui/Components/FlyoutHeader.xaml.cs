using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Components;

public partial class FlyoutHeader : ContentView, ITransientDependency
{
	public FlyoutHeader(FlyoutHeaderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = this.ViewModel = viewModel;
	}

    public FlyoutHeaderViewModel ViewModel { get; }
}