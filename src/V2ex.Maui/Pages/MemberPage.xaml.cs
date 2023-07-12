using V2ex.Maui.Pages.ViewModels;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class MemberPage : ContentPage, ITransientDependency
{
    private MemberPageViewModel ViewModel { get; }
	public MemberPage(MemberPageViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = this.ViewModel = viewModel;
	}


    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if(this.ViewModel.CurrentState == StateKeys.Success)
        {
            return;
        }
        Task.Run(() => this.ViewModel.Load());
    }
}