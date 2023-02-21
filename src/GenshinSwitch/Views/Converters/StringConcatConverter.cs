using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class StringConcatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string left)
        {
            if (parameter is string right)
            {
                return left + right;
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}
