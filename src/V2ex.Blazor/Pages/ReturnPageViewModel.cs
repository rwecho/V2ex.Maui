using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace V2ex.Blazor.Pages;

public partial class ReturnPageViewModel: ObservableObject
{
    public ReturnPageViewModel(string targetLocation)
    {
        this.TargetLocation = targetLocation;
    }

    [ObservableProperty]
    private string targetLocation, pageTitle="";

    [ObservableProperty]
    private bool hasNavigationBar = DeviceInfo.Platform != DevicePlatform.WinUI;

    [RelayCommand]
    private void OnTitleChange(string title)
    {
        this.PageTitle = title;
    }
}
