using GenshinSwitch.Fetch.Attributes;
using GenshinSwitch.Fetch.Regedit;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace GenshinSwitch.Fetch.ConfigIni;

public static class GenshinConfigIni
{
    public static string FilePath = @$"{GenshinRegedit.InstallPathCN}\config.ini";
    public static Dictionary<string, string> Launcher { get; } = new();

    static GenshinConfigIni()
    {
        Fetch();
    }

    public static void Fetch()
    {
        List<KeyValuePair<string, string>> list = 
            new ConfigurationBuilder()
                ?.AddIniFile(FilePath)
                ?.Build()
                ?.GetSection("launcher")
                ?.AsEnumerable().ToList()!;

        if (list is not null)
        {
            foreach (var kv in list)
            {
                if (kv.Value is not null)
                {
                    Launcher.Add(kv.Key, kv.Value);
                }
            }
        }
    }

    [UACRequest(ButMaybe = true)]
    public static void Save()
    {
        StringBuilder sb = new();

        sb.AppendLine("[launcher]");
        foreach (KeyValuePair<string, string> kv in Launcher)
        {
            sb.AppendLine($"{kv.Key}={kv.Value}");
        }
        File.WriteAllText(FilePath, sb.ToString());
    }
}
