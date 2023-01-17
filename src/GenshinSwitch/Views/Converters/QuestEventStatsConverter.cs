using GenshinSwitch.ViewModels.Contacts;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics.CodeAnalysis;

namespace GenshinSwitch.Views.Converters;

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "<Pending>")]
internal class QuestEventStatsColorConverter : IValueConverter
{
    private static readonly SolidColorBrush Green = new(Colors.Green);
    private static readonly SolidColorBrush OrangeRed = new(Colors.OrangeRed);
    private static readonly SolidColorBrush Gray = (Microsoft.UI.Xaml.Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush)!;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DailyNoteInfoViewModel info)
        {
            if (info.IsExtraTaskRewardReceived)
            {
                return Gray;
            }
            else
            {
                return OrangeRed;
            }
        }
        return Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
internal class QuestEventStatsColorForXboxWidgetConverter : IValueConverter
{
    private static readonly SolidColorBrush OrangeRed = new(Colors.OrangeRed);
    private static readonly SolidColorBrush Black = new(Colors.Black);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DailyNoteInfoViewModel info)
        {
            if (info.IsExtraTaskRewardReceived)
            {
                return Black;
            }
            else
            {
                return OrangeRed;
            }
        }
        return Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
internal class QuestEventStatsStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DailyNoteInfoViewModel info)
        {
            if (info.IsExtraTaskRewardReceived)
            {
                return "奖励已领取";
            }
            if (info.FinishedTaskNumber == info.TotalTaskNumber)
            {
                return "奖励未领取";
            }
            return "任务未完成";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
