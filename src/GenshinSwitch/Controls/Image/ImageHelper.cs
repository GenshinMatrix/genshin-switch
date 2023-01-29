using GenshinSwitch.Views.Converters;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Vanara.Extensions.Reflection;

namespace GenshinSwitch.Controls;

public static class ImageHelper
{
    public static double GetWidth(DependencyObject obj)
    {
        return (double)obj.GetValue(WidthProperty);
    }

    public static void SetWidth(DependencyObject obj, double value)
    {
        obj.SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(double), typeof(ImageHelper), new PropertyMetadata(0d, OnWidthChanged));

    private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Image image)
        {
            if (image.Source is BitmapImage bitmapImage)
            {
                if (!string.IsNullOrEmpty(bitmapImage.UriSource.ToString()))
                {
                    UriStringToImageSourceConverter converter = new();
                    ImageSource imageSource = converter.Convert(bitmapImage.UriSource.ToString(), typeof(ImageSource), e.NewValue, string.Empty) as ImageSource;
                    image.Source = imageSource;
                }
            }
        }
    }
}
