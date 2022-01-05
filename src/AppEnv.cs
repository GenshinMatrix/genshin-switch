using System;
using System.Diagnostics;
using System.IO;

namespace GenshinSwitch
{
    /// <summary>
    /// 程序环境管理
    /// </summary>
    public class AppEnv
    {
        /// <summary>
        /// 启动路劲
        /// </summary>
        public static string StartupPath => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// 检查运行路径下有无文件IO权限
        /// ※用于侦测程序能否在该运行目录下展开临时文件和配置文件等
        /// </summary>
        public static bool HasFilePermission
        {
            get
            {
                try
                {
                    string temp = Guid.NewGuid().ToString("N");
                    FileStream fs = new FileStream(temp, FileMode.Create);
                    fs.Close();
                    File.Delete(temp);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// OS版本信息
        /// </summary>
        public static string OSVersion
        {
            get
            {
                if (string.IsNullOrEmpty(Environment.OSVersion.ServicePack))
                {
                    return $"{Environment.OSVersion.Platform} {Environment.OSVersion.Version}";
                }
                return $"{Environment.OSVersion.Platform} {Environment.OSVersion.Version} {Environment.OSVersion.ServicePack}";
            }
        }

        /// <summary>
        /// 运行路劲是否与运行文件所在路劲一致
        /// ※用于侦测处于相对路劲的库的载入能否成功
        /// </summary>
        public static bool IsCurrentPathEqualStartupPath => Environment.CurrentDirectory == StartupPath;

        /// <summary>
        /// CD到运行文件所在目录
        /// </summary>
        public static void ChangeDirectoryToStartupPath() => Directory.SetCurrentDirectory(StartupPath);
    }

    /// <summary>
    /// 程序退出代码
    /// </summary>
    public enum EExitCode : int
    {
        /// <summary>
        /// 通用失败退出（=1）
        /// </summary>
        eFailed = '\x0001',

        /// <summary>
        /// 程序重启退出（=773）
        /// </summary>
        eRestart = 'r' + 'e' + 's' + 't' + 'a' + 'r' + 't',

        /// <summary>
        /// 程序多开退出（=1503）
        /// </summary>
        eMultiInstance = 'm' + 'u' + 'l' + 't' + 'i' + '_' + 'i' + 'n' + 's' + 't' + 'a' + 'n' + 'c' + 'e',
    }
}
