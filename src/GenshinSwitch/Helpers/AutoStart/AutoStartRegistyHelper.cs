using Microsoft.Win32;

namespace GenshinSwitch.Helpers;

/// <summary>
/// https://github.com/eservicepartner/espUrl/blob/master/espurl.win.screenshot/Infrastructure/AutoStart.cs
/// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
/// </summary>
public class AutoStartRegistyHelper : IAutoStartHelper
{
    private const string RunLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

    private readonly string keyName;
    private readonly string launchCommand;

    public AutoStartRegistyHelper()
    {
        keyName = Pack.AppName;
        launchCommand = $"\"{Environment.ProcessPath!}\" -autostart";
    }

    public void Enable()
    {
        using RegistryKey key = Registry.CurrentUser.CreateSubKey(RunLocation);
        key?.SetValue(keyName, launchCommand);
    }

    public bool IsEnabled()
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(RunLocation);

        if (key == null)
        {
            return false;
        }

        string value = (string)key.GetValue(keyName);

        if (value == null)
        {
            return false;
        }

        return value == launchCommand;
    }

    public void Disable()
    {
        using RegistryKey key = Registry.CurrentUser.CreateSubKey(RunLocation);

        if (key == null)
        {
            return;
        }

        if (key.GetValue(keyName) != null)
        {
            key.DeleteValue(keyName);
        }
    }

    public void SetEnabled(bool enable)
    {
        if (enable)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
}
