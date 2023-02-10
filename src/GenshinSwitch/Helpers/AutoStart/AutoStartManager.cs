namespace GenshinSwitch.Helpers;

public static class AutoStartManager
{
    private static IAutoStartHelper Service { get; } = new AutoStartProgramDataHelper();

    public static bool IsEnabled()
    {
        return Service.IsEnabled();
    }

    public static void SetEnabled(bool enable)
    {
        Service.SetEnabled(enable);
    }
}
