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
            RestartAsElevated('r' + 'u' + 'n' + 'a' + 's');
        }
    }

    public static void RestartAsElevated(int? exitCode = null)
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                Verb = "runas",
                UseShellExecute = true,
                FileName = Environment.ProcessPath,
                WorkingDirectory = Environment.CurrentDirectory,
            });
        }
        catch (Win32Exception)
        {
            return;
        }
        Environment.Exit(exitCode ?? 0);
    }
}
