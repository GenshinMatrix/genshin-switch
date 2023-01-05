using GenshinSwitch.Core;
using GenshinSwitch.Fetch.Regedit;
using System.Diagnostics;
using System.Security.Principal;

namespace GenshinSwitch.Fetch.Launch;

public class LaunchCtrl
{
    public const string RegionCN = "CN";
    public const string RegionOVERSEA = "OVERSEA";

    public const string ProcessNameCN = "YuanShen";
    public const string ProcessNameOVERSEA = "GenshinImpact";

    public const string FileNameCN = "YuanShen.exe";
    public const string FileNameOVERSEA = "GenshinImpact.exe";

    public const string FolderName = "Genshin Impact Game";

    public static bool TryGetProcess(out Process? process)
    {
        try
        {
            Process[] ps = Process.GetProcessesByName(ProcessNameCN);

            if (ps.Length <= 0)
            {
                ps = Process.GetProcessesByName(ProcessNameOVERSEA);
            }
            if (ps.Length > 0)
            {
                foreach (Process? p in ps)
                {
                    process = p;
                    return true;
                }
            }
        }
        catch
        {
        }
        process = null!;
        return false;
    }

    public static bool TryGetProcessRegion(out string region)
    {
        region = null!;
        try
        {
            Process[] ps = Process.GetProcessesByName(ProcessNameCN);

            if (ps.Length <= 0)
            {
                ps = Process.GetProcessesByName(ProcessNameOVERSEA);
            }
            else
            {
                region = RegionCN;
            }
            if (ps.Length > 0)
            {
                region ??= RegionOVERSEA;
                foreach (Process? p in ps)
                {
                    return true;
                }
            }
        }
        catch
        {
        }
        return false;
    }

    public static bool TryClose()
    {
        return TryGetProcess(out Process? p) && (p?.CloseMainWindow() ?? false);
    }

    public static bool TryKill()
    {
        bool got = TryGetProcess(out Process? p);
        p?.Kill();
        return got && p != null;
    }

    public static async Task LaunchAsync(int? delayMs = null, RelaunchMethods relaunchMethod = RelaunchMethods.None, LaunchParameter launchParameter = null!)
    {
        try
        {
            if (relaunchMethod switch
            {
                RelaunchMethods.Kill => await TryKillAsync(),
                RelaunchMethods.Close => await TryCloseAsync(),
                _ => false,
            })
            {
                if (!SpinWait.SpinUntil(() => TryGetProcess(out _), 15000))
                {
                    throw new GenshinSwitchException("Failed to kill Genshin Impact.");
                }
            }
        }
        catch (Exception e)
        {
            throw new GenshinSwitchException(e);
        }

        if (string.IsNullOrEmpty(GenshinRegedit.InstallPathCN))
        {
            throw new GenshinSwitchException("Genshin Impact not installed.");
        }
        else
        {
            if (delayMs != null)
            {
                await Task.Delay((int)delayMs);
            }

            string fileName = Path.Combine(GenshinRegedit.InstallPathCN, FolderName, FileNameCN);

            if (!File.Exists(fileName))
            {
                fileName = Path.Combine(GenshinRegedit.InstallPathCN, FolderName, FileNameOVERSEA);
            }

            launchParameter ??= new();

            if (string.IsNullOrEmpty(launchParameter.Region) || launchParameter.Region == RegionCN)
            {
                if (!string.IsNullOrEmpty(launchParameter.Prod))
                {
                    if (!GetElevated())
                    {
                        throw new GenshinSwitchException("Needed to run as an administrator to obtain registry write permission.");
                    }

                    if (GenshinRegedit.ProdCN != launchParameter.Prod)
                    {
                        GenshinRegedit.ProdCN = launchParameter.Prod;
                    }
                }
            }
            else if (launchParameter.Region == RegionOVERSEA)
            {
                if (!string.IsNullOrEmpty(launchParameter.Prod))
                {
                    if (!GetElevated())
                    {
                        throw new GenshinSwitchException("Needed to run as an administrator to obtain registry write permission.");
                    }

                    if (GenshinRegedit.ProdOVERSEA != launchParameter.Prod)
                    {
                        GenshinRegedit.ProdOVERSEA = launchParameter.Prod;
                    }
                }
            }

            _ = Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.Combine(GenshinRegedit.InstallPathCN, FolderName, fileName),
                Arguments = launchParameter.ToString(),
                WorkingDirectory = Path.Combine(GenshinRegedit.InstallPathCN, FolderName),
                Verb = "runas",
            });
        }
    }

    public static async Task<bool> TryGetProcessAsync(Func<Process?, Task> callback = null!)
    {
        return await Task.Run(async () =>
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(ProcessNameCN);

                if (processes.Length <= 0)
                {
                    processes = Process.GetProcessesByName(ProcessNameOVERSEA);
                }
                if (processes.Length > 0)
                {
                    foreach (Process? process in processes)
                    {
                        await callback?.Invoke(process)!;
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        });
    }

    public static async Task<bool> TryCloseAsync()
    {
        return await TryGetProcessAsync(p =>
        {
            p?.CloseMainWindow();
            return Task.CompletedTask;
        });
    }

    public static async Task<bool> TryKillAsync()
    {
        return await TryGetProcessAsync(p =>
        {
            p?.Kill();
            return Task.CompletedTask;
        });
    }

    public static bool GetElevated()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
