using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace GenshinSwitch
{
    public partial class App : Application
    {
        public static TaskbarIcon TaskbarIcon { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            TaskbarIcon = (TaskbarIcon)FindResource("Taskbar");
        }
    }
}
