using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Components;

public partial class FlyoutFooter : ContentView, ITransientDependency
{
    public FlyoutFooter(FlyoutFooterViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
    }

    private FlyoutFooterViewModel ViewModel { get; }
}
