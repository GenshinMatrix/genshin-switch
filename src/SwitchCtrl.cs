using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GenshinSwitch
{
    internal class SwitchCtrl
    {
        public static void Switch(int index)
        {
            if (index < 0)
            {
                return;
            }

            try
            {
                Process[] processes = Process.GetProcessesByName("YuanShen");

                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                    {
                        process.CloseMainWindow();
                    }
                    if (!SpinWait.SpinUntil(() => Process.GetProcessesByName("YuanShen").Length <= 0, 15000))
                    {
                        MessageBox.Show("你家原神就离谱没杀死，请手动关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            Config.Instance.Accounts[index].WriteReg();

            if (string.IsNullOrEmpty(Config.Instance.InstallPath))
            {
                MessageBox.Show("请选择原神安装路径后，才能使用自动重启功能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Path.Combine(Config.Instance.InstallPath, "Genshin Impact Game", "YuanShen.exe"),
                    Verb = "runas",
                };
                Process.Start(startInfo);
            }
        }

        /// <summary>
        /// 寻找原神安装路径
        /// </summary>
        public static string FindInstallPath()
        {
            try
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\原神");
                
                if (key == null)
                {
                    return null;
                }

                object installLocation = key.GetValue("InstallPath");

                if (installLocation != null && !string.IsNullOrEmpty(installLocation.ToString()))
                {
                    return installLocation.ToString();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            return null;
        }
    }
}
