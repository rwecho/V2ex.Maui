using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class HomeSettingPage : ContentPage, ITransientDependency
{
    public HomeSettingPage(HomeSettingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
    }

    private HomeSettingPageViewModel ViewModel { get; }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel.Load());
    }

    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {
        var label = (Label)((Element)sender).Parent;
        var definitionViewModel = label.BindingContext as TabDefinitionViewModel;
        e.Data.Properties["Source"] = definitionViewModel;
    }

    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        var source = e.Data.Properties["Source"] as TabDefinitionViewModel;
        var destination = ((Element)sender).Parent.BindingContext as TabDefinitionViewModel;

        this.ViewModel.Move(source!, destination!);
        e.Handled = true;
    }

    private void DropGestureRecognizer_DragOver(object sender, DragEventArgs e)
    {
        //e.AcceptedOperation = DataPackageOperation.None;
    }
}
