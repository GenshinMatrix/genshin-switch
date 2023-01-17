using GenshinSwitch.ViewModels.Contacts;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class LazyInfoStatsStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string message = "未知任务";

        if (value is LazyInfoViewModel info)
        {
            if (info.IsFetched)
            {
                if (info.IsFinished)
                {
                    message = "任务已完成";
                }
                else
                {
                    message = "任务未完成";
                }
            }
        }
#if false
        if (parameter is ContactProgress progress)
        {
            if (progress.IsYellow)
            {
                message = "未知任务";
            }
        }
#endif
        return message;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
