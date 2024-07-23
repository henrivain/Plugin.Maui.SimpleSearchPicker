using System.Globalization;

namespace Plugin.Maui.SimpleSearchPicker;

internal class InvertBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            return !boolean;
        }
        throw new NotImplementedException("Value type is not bool");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Convert(value, targetType, parameter, culture);
}