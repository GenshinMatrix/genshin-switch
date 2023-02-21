using CliWrap;
using GenshinSwitch.Core;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;

namespace GenshinSwitch.Fetch.Regedit;

public static class GenshinRegedit
{
    public static string InstallPathCN => GetInstallPath(GameType.CN);

    public static string ProdCN
    {
        get => GetStringFromRegedit(RegeditKeys.PROD_CN, GameType.CN);
        set => SetStringToRegedit(RegeditKeys.PROD_CN, value, GameType.CN);
    }

    public static string InstallPathOVERSEA => GetInstallPath(GameType.OVERSEA);

    public static string ProdOVERSEA
    {
        get => GetStringFromRegedit(RegeditKeys.PROD_OVERSEA, GameType.OVERSEA);
        set => SetStringToRegedit(RegeditKeys.PROD_OVERSEA, value, GameType.OVERSEA);
    }

    [Obsolete]
    public static string DataCN
    {
        get => GetStringFromRegedit(RegeditKeys.DATA, GameType.CN);
        set => SetStringToRegedit(RegeditKeys.DATA, value, GameType.CN);
    }

    [Obsolete]
    public static string DataOVERSEA
    {
        get => GetStringFromRegedit(RegeditKeys.DATA, GameType.OVERSEA);
        set => SetStringToRegedit(RegeditKeys.DATA, value, GameType.OVERSEA);
    }

    internal static string GetInstallPath(GameType type = GameType.CN)
    {
        try
        {
            using RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey? key = hklm.OpenSubKey(type.GetRegUninstallName());

            if (key == null)
            {
                key = hklm.OpenSubKey(type.GetRegUninstallName());

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

    internal static string GetStringFromRegedit(string key, GameType type = GameType.CN)
    {
#if LEGACY
        object? value = Registry.GetValue(type.GetRegKeyName(), key, string.Empty);

        if (value is byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        return null!;
#endif
        using MemoryStream stream = new();
        CommandResult result = Cli.Wrap("PowerShell")
            .WithArguments(@$"Get-ItemPropertyValue -Path 'HKCU:\Software\miHoYo\{type.ParseGameType()}' -Name '{type.GetRegKey()}';")
            .WithStandardOutputPipe(PipeTarget.ToStream(stream, true))
            .ExecuteAsync().Task.Result;
        byte[] bytes = stream.ToArray();
        string lines = Encoding.UTF8.GetString(bytes);
        StringBuilder sb = new();

        foreach (string line in lines.Replace("\r", string.Empty).Split('\n'))
        {
            if (byte.TryParse(line, out byte b))
            {
                sb.Append((char)b);
            }
        }
        Logger.Ignore(sb.ToString());
        return sb.ToString();
    }

    internal static void SetStringToRegedit(string key, string value, GameType type = GameType.CN)
    {
#if LEGACY
        Registry.SetValue(GetRegKeyName(type), key, Encoding.UTF8.GetBytes(value));
#endif
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        string script = $"""
            $value = [Convert]::FromBase64String('{base64}');
            Set-ItemProperty -Path 'HKCU:\Software\miHoYo\{type.ParseGameType()}' -Name '{type.GetRegKey()}' -Value $value -Force;
            """;
        Process.Start(new ProcessStartInfo()
        {
            FileName = "PowerShell",
            Arguments = script,
            CreateNoWindow = true,
        })?.WaitForExit();
    }

    internal static string GetRegKey(this GameType type)
    {
        return type switch
        {
            GameType.OVERSEA => RegeditKeys.PROD_OVERSEA,
            GameType.CNCloud => RegeditKeys.PROD_CNCloud,
            GameType.CN or _ => RegeditKeys.PROD_CN,
        };
    }

    internal static string GetRegKeyName(this GameType type)
    {
        return @"HKEY_CURRENT_USER\SOFTWARE\miHoYo\" + ParseGameType(type);
    }

    internal static string GetRegUninstallName(this GameType type)
    {
        return @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + ParseGameType(type);
    }

    internal static string ParseGameType(this GameType type)
    {
        return type switch
        {
            GameType.OVERSEA => RegeditKeys.OVERSEA,
            GameType.CNCloud => RegeditKeys.CNCloud,
            GameType.CN or _ => RegeditKeys.CN,
        };
    }
}

internal enum GameType
{
    CN,
    OVERSEA,
    CNCloud,
}
