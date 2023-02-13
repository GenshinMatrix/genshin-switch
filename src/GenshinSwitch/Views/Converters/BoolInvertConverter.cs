using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class BoolInvertConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool @bool)
        {
            return !@bool;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool @bool)
        {
            return !@bool;
        }
        return value;
    }
}
