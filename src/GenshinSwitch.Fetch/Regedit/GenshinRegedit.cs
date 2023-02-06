using GenshinSwitch.Core;
using Microsoft.Win32;
using System.Text;

namespace GenshinSwitch.Fetch.Regedit;

public class GenshinRegedit
{
    public static string InstallPathCN => GetInstallPath(false);

    public static string ProdCN
    {
        get => GetStringFromRegedit(RegeditKeys.PROD_CN, false);
        set => SetStringToRegedit(RegeditKeys.PROD_CN, value, false);
    }

    public static string DataCN
    {
        get => GetStringFromRegedit(RegeditKeys.DATA, false);
        set => SetStringToRegedit(RegeditKeys.DATA, value, false);
    }

    public static string InstallPathOVERSEA => GetInstallPath(true);

    public static string ProdOVERSEA
    {
        get => GetStringFromRegedit(RegeditKeys.PROD_OVERSEA, true);
        set => SetStringToRegedit(RegeditKeys.PROD_OVERSEA, value, true);
    }

    public static string DataOVERSEA
    {
        get => GetStringFromRegedit(RegeditKeys.DATA, true);
        set => SetStringToRegedit(RegeditKeys.DATA, value, true);
    }

    internal static string GetInstallPath(bool oversea = false)
    {
        try
        {
            using RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey? key = hklm.OpenSubKey(GetRegUninstallName(oversea));

            if (key == null)
            {
                key = hklm.OpenSubKey(GetRegUninstallName(true));

                if (key == null)
                {
                    return null!;
                }
            }

            object installLocation = key.GetValue("InstallPath")!;
            key?.Dispose();

            if (installLocation != null && !string.IsNullOrEmpty(installLocation.ToString()))
            {
                return installLocation.ToString()!;
            }
        }
        catch (Exception e)
        {
            throw new GenshinSwitchException(e);
        }
        return null!;
    }

    internal static string GetStringFromRegedit(string key, bool oversea = false)
    {
        object? value = Registry.GetValue(GetRegKeyName(oversea), key, string.Empty);

        if (value is byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        return null!;
    }

    internal static void SetStringToRegedit(string key, string value, bool oversea = false)
    {
        Registry.SetValue(GetRegKeyName(oversea), key, Encoding.UTF8.GetBytes(value));
    }

    internal static string GetRegKeyName(bool oversea = false)
    {
        return @"HKEY_CURRENT_USER\SOFTWARE\miHoYo\" + (oversea ? "Genshin Impact" : "原神");
    }

    internal static string GetRegUninstallName(bool oversea = false)
    {
        return @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + (oversea ? "Genshin Impact" : "原神");
    }
}
