using System.Windows.Input;

namespace V2ex.Blazor.Pages;

public static class WebViewEvents
{
    public static readonly BindableProperty TitleChangeCommandProperty =
       BindableProperty.CreateAttached("TitleChangeCommand", typeof(ICommand), typeof(WebViewEvents), null);

    public static ICommand GetTitleChangeCommand(BindableObject target)
    {
        return (ICommand)target.GetValue(TitleChangeCommandProperty);
    }

    public static void SetTitleChangeCommand(BindableObject target, ICommand value)
    {
        target.SetValue(TitleChangeCommandProperty, value);
    }
}
