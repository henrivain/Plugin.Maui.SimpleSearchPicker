using System.Globalization;

namespace Plugin.Maui.SimpleSearchPicker;
internal class SelectedItemConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string? placeholder = null;
        foreach (var value in values)
        {
            if (value is IStringPresentable presentable)
            {
                return presentable.VisibleData;
            }
            if (value is string str)
            {
                return str;
            }
            if (value is PlaceholderString placeholderObj)
            {
                placeholder = placeholderObj.Value;
            }
        }
        return placeholder ?? "search term";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
