using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace GenshinSwitch.WindowsService;

internal static class MainRegister
{
    public static bool Register()
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(GetNet4xInstallPath(), "InstallUtil.exe"),
                Arguments = $"{Process.GetCurrentProcess().MainModule.FileName}",
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas",
            },
        };
        p.Start();
        p.WaitForExit();
        return p.ExitCode == 0;
    }

    public static bool Unregister()
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(GetNet4xInstallPath(), "InstallUtil.exe"),
                Arguments = $"/u \"{Process.GetCurrentProcess().MainModule.FileName}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas",
            },
        };
        p.Start();
        p.WaitForExit();
        return p.ExitCode == 0;
    }

    public static string? GetNet4xInstallPath()
    {
        using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");

        if (key != null)
        {
            object? installPath = key.GetValue("InstallPath");
            return installPath?.ToString();
        }
        return null!;
    }
}
