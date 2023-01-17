using GenshinSwitch.ViewModels.Contacts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics.CodeAnalysis;

namespace GenshinSwitch.Views.Converters;

internal class TreasurebowCoinsIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        int? w = null;
        Uri? uri = null;

        if (parameter is string pixel)
        {
            w = int.Parse(pixel);
        }

        if (value is DailyNoteInfoViewModel info)
        {
            if (info.MaxHomeCoin == 0)
            {
                return null!;
            }

            double ratio = info.CurrentHomeCoin / (double)info.MaxHomeCoin;

            if (ratio == 0d)
            {
                uri = null!;
            }
            else if (ratio >= 0.666d)
            {
                uri = new Uri("ms-appx:///Assets/Images/UI_HomeworldLevel_Coins_2.png");
            }
            else if (ratio >= 0.333d)
            {
                uri = new Uri("ms-appx:///Assets/Images/UI_HomeworldLevel_Coins_1.png");
            }
            else
            {
                uri = new Uri("ms-appx:///Assets/Images/UI_HomeworldLevel_Coins_3.png");
            }
        }

        if (uri != null)
        {
            if (w != null)
            {
                BitmapImage image = new()
                {
                    DecodePixelWidth = w.Value,
                    UriSource = uri,
                };
                return image;
            }
            else
            {
                return new BitmapImage(uri);
            }
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null!;
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
internal class TreasurebowCoinsMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DailyNoteInfoViewModel info)
        {
            if (info.MaxHomeCoin == 0)
            {
                return new Thickness(0d);
            }

            double ratio = info.CurrentHomeCoin / (double)info.MaxHomeCoin;

            if (ratio == 0d)
            {
                return new Thickness(0d);
            }
            else if (ratio >= 0.666d)
            {
                return new Thickness(0d, -111d, 0d, 0d);
            }
            else if (ratio >= 0.333d)
            {
                return new Thickness(0d, -100d, 0d, 0d);
            }
            else
            {
                return new Thickness(0d, -102d, 0d, 0d);
            }
        }
        return new Thickness(0d);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null!;
    }
}
