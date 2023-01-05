using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool @bool)
        {
            return @bool ? Visibility.Visible : Visibility.Collapsed;
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null!;
    }
}
