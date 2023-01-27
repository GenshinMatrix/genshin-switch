using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class QuestRandomProceIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int icon)
        {
            if (icon.ToString() == parameter?.ToString())
            {
                return Visibility.Visible;
            }
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null!;
    }
}
