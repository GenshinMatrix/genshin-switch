using Microsoft.UI.Xaml;

namespace GenshinSwitch.Models.Messages;

internal class ThemeChangedMessage
{
    public ElementTheme? Theme { get; set; }
    public string? Backdrop { get; set; }
}
