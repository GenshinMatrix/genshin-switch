using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace GenshinSwitch
{
    public partial class App : Application
    {
        public static TaskbarIcon TaskbarIcon { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            TaskbarIcon = (TaskbarIcon)FindResource("Taskbar");
        }
    }
}
