using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinSwitch.WindowsService;

internal class RuntimeHelper
{
    public static bool IsElevated { get; } = GetElevated();
    public static bool IsDebuggerAttached => Debugger.IsAttached;
    public static bool IsDesignMode { get; } = GetDesignMode();

    private static bool GetElevated()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static bool GetDesignMode()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return true;
        }
        else if (Process.GetCurrentProcess().ProcessName == "devenv")
        {
            return true;
        }
        return false;
    }

    public static void EnsureElevated()
    {
        if (IsDebuggerAttached)
        {
            return;
        }
        if (!IsElevated)
        {
            RestartAsElevated();
        }
    }

    public static void RestartAsElevated(int? exitCode = null, bool forced = false)
    {
        static string ReArguments()
        {
            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();

            for (int i = default; i < args.Length; i++)
            {
                args[i] = $@"""{args[i]}""";
            }
            return string.Join(" ", args);
        }

        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                Verb = "runas",
                Arguments = ReArguments(),
                UseShellExecute = true,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                WorkingDirectory = Environment.CurrentDirectory,
            });
        }
        catch (Win32Exception)
        {
            return;
        }
        if (forced)
        {
            Process.GetCurrentProcess().Kill();
        }
        Environment.Exit(exitCode ?? 'r' + 'u' + 'n' + 'a' + 's');
    }

    public static void CheckSingleInstance()
    {
        EventWaitHandle? handle;

        try
        {
            handle = EventWaitHandle.OpenExisting("GenshinSwitch.WindowsService");
            handle.Set();
            Environment.Exit(0xFFFF);
        }
        catch (WaitHandleCannotBeOpenedException)
        {
            handle = new EventWaitHandle(false, EventResetMode.AutoReset, "GenshinSwitch.WindowsService");
        }
        GC.KeepAlive(handle);
        _ = Task.Run(() =>
        {
            while (handle.WaitOne())
            {
            }
        });
    }
}
