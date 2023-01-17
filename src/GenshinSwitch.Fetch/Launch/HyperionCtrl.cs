using GenshinSwitch.Core;
using System.Diagnostics;

namespace GenshinSwitch.Fetch.Launch;

public static class HyperionCtrl
{
    /// <summary>
    /// Windows Subsystem for Android™️
    /// Windows 11 Only Android Hoyolab Application
    /// </summary>
    public const string Android = "wsa://com.mihoyo.hyperion";

    /// <summary>
    /// Chinese Server Hoyolab named MiYouShe
    /// </summary>
    public const string Web = "https://www.miyoushe.com/ys/";

    public static async Task LaunchAsync(string type = null!)
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = type ?? Android,
            });
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        await Task.CompletedTask;
    }
}
