using GenshinSwitch.ViewModels.Contacts;
using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class HoyolabSignInStatsStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string message = "未知签到";

        if (value is SignInInfoViewModel info)
        {
            if (info.IsFetched)
            {
                if (info.IsSign)
                {
                    message = "今天已签到";
                }
                else
                {
                    message = "今天未签到";
                }
            }
        }
        if (parameter is ContactProgress progress)
        {
            if (progress.IsYellow)
            {
                message = "未知签到";
            }
        }
        return message;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
