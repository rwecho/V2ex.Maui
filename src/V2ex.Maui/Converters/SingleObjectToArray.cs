using System.Globalization;
using V2ex.Maui.Services;

namespace V2ex.Maui.Converters;

public class SingleObjectToArray : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Array.Empty<object>();
        }
        return new object[] { value };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class NightModeBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var parameterNightMode = int.Parse(parameter?.ToString() ?? "0");
        if (value is NightMode nightMode)
        {
            return nightMode == (NightMode)parameterNightMode;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var parameterNightMode = int.Parse(parameter?.ToString() ?? "0");
        if (value is bool boolean && boolean)
        {
            return (NightMode)parameterNightMode;
        }
        return null;
    }
}