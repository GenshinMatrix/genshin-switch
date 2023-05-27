using GenshinSwitch.Fetch.Lazy;
using GenshinSwitch.Models;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.Threading;
using WinRT.Interop;

namespace GenshinSwitch.Views;

public sealed partial class LaunchWindow : WindowEx
{
    public IntPtr Hwnd { get; private set; }

    public LaunchWindow()
    {
        Hwnd = WindowNative.GetWindowHandle(this);
        InitializeComponent();
#if LEGACY
        Backdrop = Settings.Backdrop.Get() switch
        {
            "None" => null,
            "Acrylic" => new AcrylicSystemBackdrop(),
            "Mica" or _ => new MicaSystemBackdrop(),
        };
#else
        SystemBackdrop = Settings.Backdrop.Get() switch
        {
            "None" => null,
            "Acrylic" => new DesktopAcrylicBackdrop(),
            "Mica" or _ => new MicaBackdrop(),
        };
#endif
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "../Assets/Logos/Favicon.ico"));

        Task.Run(async () =>
        {
            await LazyLauncher.LaunchAsync(Settings.ComponentLazyPath.Get());
        }).Forget();
    }
}
