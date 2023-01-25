using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace GenshinSwitch;

public sealed partial class MainWindow : WindowEx
{
    public IntPtr Hwnd { get; private set; }
    public XamlRoot XamlRoot => Content.XamlRoot;
    public ElementTheme ActualTheme => ((FrameworkElement)Content).ActualTheme;

    public MainWindow()
    {
        Hwnd = WindowNative.GetWindowHandle(this);
        InitializeComponent();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Logos/Favicon.ico"));
        Content = null;
        SetupBackdrop();
        Title = "AppDisplayName".GetLocalized();

        WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (_, _) => SetupBackdrop());
    }

    private void SetupBackdrop()
    {
        Backdrop = Settings.Backdrop.Get() switch
        {
            "None" => null,
            "Acrylic" => new AcrylicSystemBackdrop(),
            "Mica" or _ => new MicaSystemBackdrop(),
        };
    }
}
