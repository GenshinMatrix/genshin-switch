using Microsoft.UI.Dispatching;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Vanara.PInvoke;

namespace GenshinSwitch.Helpers;

public class RuntimeHelper
{
    [DllImport("Microsoft.ui.xaml.dll")]
    public static extern void XamlCheckProcessRequirements();

    public static bool IsElevated { get; } = GetElevated();
    public static bool IsMSIX { get; } = GetMSIX();
    public static bool IsDebuggerAttached => Debugger.IsAttached;
    public static bool IsDesignMode { get; } = GetDesignMode();
    public static bool IsWin11 => Environment.OSVersion.Version.Build >= 22000;

    private static bool GetMSIX()
    {
        uint packageFullNameLength = default;
        return (long)Kernel32.GetCurrentPackageFullName(ref packageFullNameLength, null!) != 15700L;
    }

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
        if (IsDebuggerAttached && IsMSIX)
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
            string[] args = Environment.GetCommandLineArgs()[1..^0];

            for (int i = default; i < args.Length; i++)
            {
                args[i] = $@"""{args[i]}""";
            }
            return string.Join(' ', args);
        }

        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                Verb = "runas",
                Arguments = ReArguments(),
                UseShellExecute = true,
                FileName = Environment.ProcessPath,
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
            handle = EventWaitHandle.OpenExisting(Pack.AppName);
            handle.Set();
            Environment.Exit(0xFFFF);
        }
        catch (WaitHandleCannotBeOpenedException)
        {
            handle = new EventWaitHandle(false, EventResetMode.AutoReset, Pack.AppName);
        }
        GC.KeepAlive(handle);
        _ = Task.Run(() =>
        {
            while (handle.WaitOne())
            {
                App.MainWindow?.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
                {
                    App.MainWindow?.Activate();
                    App.MainWindow?.Show();
                });
            }
        });
    }
}
