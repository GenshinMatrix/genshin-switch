using GenshinSwitch.ViewModels.Contacts;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics.CodeAnalysis;

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
        return message;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
internal class LazyInfoStatsColorConverter : IValueConverter
{
    private static readonly SolidColorBrush Green = new(Colors.Green);
    private static readonly SolidColorBrush OrangeRed = new(Colors.OrangeRed);
    private static SolidColorBrush Gray => (Microsoft.UI.Xaml.Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush)!;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is LazyInfoViewModel info)
        {
            if (info.IsFetched)
            {
                if (info.IsFinished)
                {
                    return Green;
                }
                else
                {
                    return OrangeRed;
                }
            }
        }
        return Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
