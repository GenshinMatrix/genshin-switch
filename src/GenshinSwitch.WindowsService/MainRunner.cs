using System.Diagnostics;

namespace GenshinSwitch.WindowsService;

internal static class MainRunner
{
    public static bool Start()
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "net.exe",
                Arguments = "start GenshinSwitch.WindowsService",
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas",
            },
        };
        p.Start();
        p.WaitForExit();
        return p.ExitCode == 0;
    }

    public static bool Stop()
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "net.exe",
                Arguments = "stop GenshinSwitch.WindowsService",
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas",
            },
        };
        p.Start();
        p.WaitForExit();
        return p.ExitCode == 0;
    }
}
