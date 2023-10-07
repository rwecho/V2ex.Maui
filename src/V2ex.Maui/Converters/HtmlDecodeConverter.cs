using System.Globalization;
using System.Web;
using V2ex.Maui.Services;

namespace V2ex.Maui.Converters;
public class HtmlDecodeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            return HttpUtility.HtmlDecode(text);
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
