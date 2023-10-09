using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Components;

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