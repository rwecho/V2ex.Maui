using System.Globalization;
using V2ex.Maui.Services;

namespace V2ex.Maui.Converters;

public class HtmlContainerConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string html)
        {
            return value;
        }
        var htmlContainer = AppStateManager.AppState.HtmlContainer;
        return htmlContainer == null ? html : htmlContainer.Replace("@html", html);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
