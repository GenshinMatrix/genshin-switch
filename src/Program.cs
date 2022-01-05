using System;
using System.Diagnostics;
using System.Reflection;

namespace GenshinSwitch
{
    public class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            // 初始化程序单例名：服务于后续各类检查动作
            AppWrapper.InitInstanceName(MethodBase.GetCurrentMethod().DeclaringType);

            // 检查程序是否允许多开
            if (!AppWrapper.IsMutilInstanceEnabled && AppWrapper.IsAnotherInstanceStarted)
            {
                Trace.WriteLine("Another instance was already running.");

                // 连接到跨进程通讯IPC通道
                AppIpcObject client = AppIpc.CreateIpcClientChannel(AppWrapper.InstanceName);

                // 恢复并激活已启动的窗体
                client?.RestoreAndActivate();

                Environment.ExitCode = (int)EExitCode.eMultiInstance;
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                // 创建跨进程通讯IPC通道
                AppIpc.CreateIpcServerChannel(AppWrapper.InstanceName);
            }

            // 显示启动界面
            //SplashScreen.Show();

            App app = new App();
            app.Run();
        }
    }
}
