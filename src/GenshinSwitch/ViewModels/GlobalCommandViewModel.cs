using CommunityToolkit.Mvvm.Input;
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
}
