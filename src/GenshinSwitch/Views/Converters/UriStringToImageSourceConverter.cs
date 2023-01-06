using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace GenshinSwitch.Views.Converters;

internal class UriStringToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        int? w = null;
        Uri? uri = null;

        if (parameter is string pixel)
        {
            w = int.Parse(pixel);
        }

        if (value is Uri _uri)
        {
            uri = _uri;
        }
        else if (value is string uriString)
        {
            uri = new Uri(uriString);
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
