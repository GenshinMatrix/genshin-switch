using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Core;
using GenshinSwitch.Fetch.Muter;
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
        SetupBackdrop();
        InitializeComponent();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Logos/Favicon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        MuteManager.AutoMute = Settings.AutoMute;
        WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (_, _) => SetupBackdrop());

        AppWindow.Closing += (_, e) =>
        {
            switch (Settings.CloseButtonMethod)
            {
                case 0:
                    e.Cancel = true;
                    AppWindow.Hide();
                    break;
                case 1:
                    (App.Current as App)?.ExitForce();
                    break;
            }
        };

        bool autostart = false;
        VisibilityChanged += (_, _) =>
        {
            if (autostart)
            {
                return;
            }

            try
            {
                if (AppWindow.IsVisible && CommandLineHelper.Has("autostart"))
                {
                    autostart = true;
                    AppWindow.Hide();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        };
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
