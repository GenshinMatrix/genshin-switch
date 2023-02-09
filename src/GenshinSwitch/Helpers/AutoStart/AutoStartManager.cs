namespace GenshinSwitch.Helpers;

#pragma warning disable CS0162

public static class AutoStartManager
{
    private const AutoStartType Type = AutoStartType.ProgramData;
    private static AutoStartHelper RegistyService => new(Pack.AppName, $"{Environment.ProcessPath!}");
    private static AutoStartupHelper ProgramDataService => new();

    public static bool IsEnabled()
    {
        if (Type == AutoStartType.ProgramData)
        {
            return ProgramDataService.IsEnabled();
        }
        return RegistyService.IsEnabled();
    }

    public static void SetEnabled(bool enable)
    {
        if (Type == AutoStartType.ProgramData)
        {
            ProgramDataService.SetEnabled(enable);
            return;
        }
        RegistyService.SetEnabled(enable);
    }

    private enum AutoStartType
    {
        /// <summary>
        /// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
        /// </summary>
        Registy,

        /// <summary>
        /// C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup
        /// </summary>
        ProgramData,

        /// <summary>
        /// C:\Users\{userName}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
        /// </summary>
        AppDataRoaming,
    }
}
