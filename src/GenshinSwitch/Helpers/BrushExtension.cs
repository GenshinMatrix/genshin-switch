using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace GenshinSwitch.Helpers;

public static class BrushExtension
{
    public static Brush ToBrush(this Color color)
    {
        Brush brush = new SolidColorBrush(color);
        return brush;
    }
}
