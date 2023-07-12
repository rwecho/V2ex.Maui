using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class NotificationsPage : ContentPage, ITransientDependency
{
	public NotificationsPage()
	{
		InitializeComponent();
	}
}