using System.Globalization;

namespace MauiExtension.SimpleSearchPicker;
internal class SelectedItemConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return "search term";
        }
        if (value is IStringPresentable presentable)
        {
            return presentable.VisibleData;
        }
        throw new NotImplementedException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
