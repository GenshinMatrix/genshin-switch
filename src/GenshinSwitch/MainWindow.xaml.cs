using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
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
        Backdrop = Settings.Backdrop.Get() switch
        {
            "None" => null,
            "Acrylic" => new AcrylicSystemBackdrop(),
            "Mica" or _ => new MicaSystemBackdrop(),
        };
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Logos/Favicon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        //VisibilityChanged += (_, _) =>
        //{
        //    IntPtr result = User32.SendMessage(WindowNative.GetWindowHandle(this), (uint)User32.ButtonMessage.BCM_SETSHIELD, IntPtr.Zero, (IntPtr)1);

        //    if (result != IntPtr.Zero)
        //    {
        //    }
        //};
    }
}
