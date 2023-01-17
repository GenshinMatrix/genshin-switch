using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class BoolToVisibilityReversedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool @bool)
        {
            return @bool ? Visibility.Collapsed : Visibility.Visible;
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
