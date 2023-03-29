using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Text;

namespace GenshinSwitch.WindowsService;

internal static class CommandRunner
{
    public static dynamic? Run(dynamic? obj)
    {
        _ = obj ?? JsonConvert.DeserializeObject(
            """
            {
                "Command": 1,
                "Type": "CN",
                "Key": "",
                "Value": "",
            }
            """
        );

        if (obj != null)
        {
            if ((int)obj.Command == (int)MainServiceCommmand.SetGameAccountRegisty)
            {
#if true
                Registry.SetValue(((GameType)Enum.Parse(typeof(GameType), (string)obj.Type)).GetRegKeyName(), (string)obj.Key, Encoding.UTF8.GetBytes((string)obj.Value));
#else
                try
                {
                    string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes((string)obj.Value));
                    string script = $"""
                    $value = [Convert]::FromBase64String('{base64}');
                    Set-ItemProperty -Path 'HKCU:\Software\miHoYo\{((GameType)Enum.Parse(typeof(GameType), (string)obj.Type)).ParseGameType()}' -Name '{(string)obj.Key}' -Value $value -Force;
                    """;
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = "powershell",
                        Arguments = script,
                        CreateNoWindow = true,
                        WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        Verb = "runas",
                    })?.WaitForExit();
                }
                catch
                {
                }
#endif
            }
            else if ((int)obj.Command == (int)MainServiceCommmand.GetGameAccountRegisty)
            {
                object? value = Registry.GetValue(((GameType)Enum.Parse(typeof(GameType), (string)obj.Type)).GetRegKeyName(), (string)obj.Key, string.Empty);

                return new
                {
                    Value = (value is byte[] bytes) ? Convert.ToBase64String(bytes) : string.Empty,
                    obj.Command,
                    obj.Type,
                    obj.Key,
                };
            }
        }
        return null!;
    }
}

file enum MainServiceCommmand
{
    None = 0x00,
    SetGameAccountRegisty = 0x01,
    GetGameAccountRegisty = 0x02,
}

file enum GameType
{
    CN,
    OVERSEA,
    CNCloud,
}

file static class RegeditKeys
{
    public const string CN = "原神";
    public const string PROD_CN = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
    public const string DATA = "GENERAL_DATA_h2389025596";

    public const string OVERSEA = "Genshin Impact";
    public const string PROD_OVERSEA = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";

    public const string CNCloud = "云·原神";
    public const string PROD_CNCloud = "MIHOYOSDK_ADL_0";

    public static string GetRegKeyName(this GameType type)
    {
        return @"HKEY_CURRENT_USER\SOFTWARE\miHoYo\" + ParseGameType(type);
    }

    public static string ParseGameType(this GameType type)
    {
        return type switch
        {
            GameType.OVERSEA => OVERSEA,
            GameType.CNCloud => CNCloud,
            GameType.CN or _ => CN,
        };
    }
}
