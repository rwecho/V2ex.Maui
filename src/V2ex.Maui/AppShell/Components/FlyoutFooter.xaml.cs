using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.AppShell.Components;

public partial class FlyoutFooter : ContentView, ITransientDependency
{
    public FlyoutFooter(FlyoutFooterViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
    }

    private FlyoutFooterViewModel ViewModel { get; }
}

public partial class FlyoutFooterViewModel : ObservableObject,  ITransientDependency
{
    public FlyoutFooterViewModel(AssemblyService assemblyService)
    {
        this.AssemblyService = assemblyService;
        this.Version = this.AssemblyService.GetVersion();
    }

    private AssemblyService AssemblyService { get; }

    [ObservableProperty]
    private string _version;
}