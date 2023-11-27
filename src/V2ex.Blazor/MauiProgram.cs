using Microsoft.Extensions.Logging;
using V2ex.Blazor.Services;

namespace V2ex.Blazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorShared();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddTransient<Services.IBrowser, Services.NativeBrowser>();
        builder.Services.AddTransient<Api.IPreferences, MauiPreferences>();
        builder.Services.AddScoped<INavigationInterceptorService, NavigationInterceptorService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
