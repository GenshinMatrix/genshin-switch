namespace GenshinSwitch.Helpers;

/// <summary>
/// https://github.com/eservicepartner/espUrl/blob/master/espurl.win.screenshot/Infrastructure/SettingsManager.cs
/// </summary>
public static class AutoStartManager
{
    private static readonly AutoStartHelper Service = new(Pack.AppName, $"{Environment.ProcessPath!}");

    public static bool IsEnabled()
    {
        return Service.IsEnabled();
    }

    public static void SetEnabled(bool enable)
    {
        Service.SetEnabled(enable);
    }
}
