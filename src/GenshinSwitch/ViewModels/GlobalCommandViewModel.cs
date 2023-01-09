using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Fetch.Launch;
using GenshinSwitch.Helpers;
using Microsoft.UI.Xaml;
using System.Windows.Input;
using Windows.UI.Notifications;

namespace GenshinSwitch.ViewModels;

public class GlobalCommandViewModel
{
    public static ICommand ExitAppCommand => new RelayCommand(() =>
    {
        //if (e.Parameter is TaskbarIcon taskBarIconApp)
        //    taskBarIconApp.Dispose(); // Ensure the tray icon is removed.

        ToastNotificationManager.History.Clear();
        Application.Current.Exit();
    });

    public static ICommand LaunchHyperionCommand => new RelayCommand(async () =>
    {
        if (RuntimeHelper.IsWin11 && false)
        {
            await HyperionCtrl.LaunchAsync();
        }
        else
        {
            NoticeService.AddNotice("Windows Subsystem for Android™️ NOT installed", "Hoyolab application requested OS higher than Windows 11.");
        }
    });
}
