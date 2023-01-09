using GenshinSwitch.Core;
using System.Diagnostics;

namespace GenshinSwitch.Fetch.Launch;

/// <summary>
/// Windows Subsystem for Android™️
/// Windows 11 Only Android Hoyolab Application APIs
/// </summary>
public static class HyperionCtrl
{
    public static async Task LaunchAsync()
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "wsa://com.mihoyo.hyperion",
            });
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        await Task.CompletedTask;
    }
}
