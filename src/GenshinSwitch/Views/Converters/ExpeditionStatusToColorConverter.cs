using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace GenshinSwitch.Views.Converters;

internal class ExpeditionStatusToColorConverter : IValueConverter
{
    private static readonly SolidColorBrush Green = new(Colors.Green);
    private static readonly SolidColorBrush Orange = new(Colors.Orange);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var status = (bool)value;
        if (status)
        {
            return Green;
        }
        else
        {
            return Orange;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
