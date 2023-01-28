using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Controls;

public sealed partial class MessageBoxX : ContentDialog
{
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxX), new PropertyMetadata(null!));

    public MessageBoxX()
    {
        InitializeComponent();
    }

    public MessageBoxX(string message, string title = null!)
        : this()
    {
        XamlRoot = App.MainWindow.XamlRoot;
        RequestedTheme = App.MainWindow.ActualTheme;
        Message = message;
        Title = title;
    }
}
