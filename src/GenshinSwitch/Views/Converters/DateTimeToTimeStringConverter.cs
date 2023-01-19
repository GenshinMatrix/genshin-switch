using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class DateTimeToTimeStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset time)
        {
            return time.ToString("HH:mm:ss.fff");
        }
        if (value is DateTime time1)
        {
            return time1.ToString("HH:mm:ss.fff");
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
