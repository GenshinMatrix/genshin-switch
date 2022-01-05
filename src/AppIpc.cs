using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows;
using System.Windows.Interop;

namespace GenshinSwitch
{
    public class AppIpc
    {
        /// <summary>
        /// 创建IPC服务通道
        /// </summary>
        public static void CreateIpcServerChannel(string instanceName)
        {
            _ = instanceName ?? throw new ArgumentNullException(nameof(instanceName));
            IpcServerChannel channel = new IpcServerChannel(instanceName);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(AppIpcObject), $"{nameof(AppIpcObject)}.rem", WellKnownObjectMode.SingleCall);
        }

        /// <summary>
        /// 创建IPC客户通道
        /// </summary>
        public static AppIpcObject CreateIpcClientChannel(string instanceName)
        {
            _ = instanceName ?? throw new ArgumentNullException(nameof(instanceName));
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, false);
            AppIpcObject AppIpc = Activator.GetObject(typeof(AppIpcObject), $"ipc://{instanceName}/{nameof(AppIpcObject)}.rem") as AppIpcObject;
            return AppIpc;
        }
    }

    /// <summary>
    /// 程序进程间IPC通讯资源
    /// </summary>
    public class AppIpcObject : MarshalByRefObject
    {
        /// <summary>
        /// 恢复窗体
        /// </summary>
        public void Restore()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow != null)
                {
                    if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                    {
                        Trace.WriteLine("Application main form restore requested.");

                        IntPtr handle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
                        Win32Helper.RestoreWindow(handle);
                    }
                }
            });
        }

        /// <summary>
        /// 激活窗体
        /// </summary>
        public void Activate()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow != null)
                {
                    Trace.WriteLine("Application main form activate requested.");
                    Application.Current.MainWindow.Activate();
                }
            });
        }

        /// <summary>
        /// 恢复并激活窗体
        /// </summary>
        public void RestoreAndActivate()
        {
            Restore();
            Activate();
        }

        private class Win32Helper
        {
            private const int WM_SYSCOMMAND = 0x0112;
            private const int SC_RESTORE = 0xF120;

            /// <summary>
            /// 发送消息
            /// </summary>
            [DllImport("user32.dll", EntryPoint = "SendMessage")]
            private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

            /// <summary>
            /// 恢复窗体
            /// </summary>
            internal static void RestoreWindow(IntPtr handle) => SendMessage(handle, WM_SYSCOMMAND, SC_RESTORE, 0);
        }
    }
}
