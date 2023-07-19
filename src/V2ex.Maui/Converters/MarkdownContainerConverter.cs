using System.Globalization;

namespace V2ex.Maui.Converters;

public class MarkdownContainerConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string text && parameter is string html)
        {
            return html.Replace("@markdown", text);
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
