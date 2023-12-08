namespace V2ex.Blazor;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();


        // The SoftInput in the MainActivity.cs is not working, so we have to do it here.
        // reference: https://github.com/dotnet/maui/issues/18041
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
#if ANDROID
            var window = handler.PlatformView.Window!;
            window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
#endif
        });
    }
}
