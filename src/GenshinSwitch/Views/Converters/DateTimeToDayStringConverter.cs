using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class DateTimeToDayStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset time)
        {
            return time.ToString("yyyy.MM.dd");
        }
        if (value is DateTime time1)
        {
            return time1.ToString("yyyy.MM.dd");
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
