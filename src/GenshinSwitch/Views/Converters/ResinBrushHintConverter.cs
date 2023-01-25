using CommunityToolkit.WinUI.Helpers;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class ResinBrushHintConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value != null)
        {
            parameter ??= $"{Settings.HintResinLimit.Get()};80FF0000;803C8CF0";

            if (parameter is string ps)
            {
                string[] pps = ps.Split(';');

                if (pps.Length >= 3)
                {
                    double v1 = System.Convert.ToDouble(value.ToString());
                    double v2 = System.Convert.ToDouble(pps[0]);

                    return ("#" + (v1 >= v2 ? pps[1] : pps[2])).ToColor().ToBrush();
                }
            }
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null!;
    }
}
