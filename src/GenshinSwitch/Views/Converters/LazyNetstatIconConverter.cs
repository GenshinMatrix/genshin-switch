using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class LazyNetstatIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string tag = parameter as string;

        if (value is long testLazyElapsedMilliseconds)
        {
            if (testLazyElapsedMilliseconds == -1)
            {
                return tag switch
                {
                    "1" => Visibility.Collapsed,
                    "2" => Visibility.Visible,
                    "3" => Visibility.Collapsed,
                    "x" or _ => Visibility.Visible,
                };
            }
            else if (testLazyElapsedMilliseconds > 10000)
            {
                return tag switch
                {
                    "1" => Visibility.Collapsed,
                    "2" => Visibility.Collapsed,
                    "3" => Visibility.Visible,
                    "x" or _ => Visibility.Collapsed,
                };
            }
            else if (testLazyElapsedMilliseconds > 5000)
            {
                return tag switch
                {
                    "1" => Visibility.Collapsed,
                    "2" => Visibility.Visible,
                    "3" => Visibility.Collapsed,
                    "x" or _ => Visibility.Collapsed,
                };
            }
            else
            {
                return tag switch
                {
                    "1" => Visibility.Visible,
                    "2" => Visibility.Collapsed,
                    "3" => Visibility.Collapsed,
                    "x" or _ => Visibility.Collapsed,
                };
            }
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
