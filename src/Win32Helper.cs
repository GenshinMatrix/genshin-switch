using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GenshinSwitch
{
    internal static class Win32Helper
    {
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_RESTORE = 0xF120;

        internal static void RestoreWindow(IntPtr handle) => SendMessage(handle, WM_SYSCOMMAND, SC_RESTORE, 0);

        public static void Restore(this Window mainWindow)
        {
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                IntPtr handle = new WindowInteropHelper(mainWindow).Handle;
                RestoreWindow(handle);
            }
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr handle, int msg, uint wParam, uint lParam);
    }
}
