using Microsoft.Extensions.Localization;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class MyFavoritePage : ContentPage, ITransientDependency
{
	public MyFavoritePage(IStringLocalizer<MauiResource> localizer)
	{
		InitializeComponent();

		this.Content = InstanceActivator.Create<MyFollowingPage>().Content;
	}
}