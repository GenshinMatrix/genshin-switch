using IWshRuntimeLibrary;

namespace GenshinSwitch.Helpers;

/// <summary>
/// https://www.cnblogs.com/conexpress/p/ShortcutCreator.html
/// </summary>
internal static class ShortcutCreator
{
    public static void CreateShortcut(string directory, string shortcutName, string targetPath, string arguments = null!, string description = null!, string iconLocation = null!)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
 
        string shortcutPath = Path.Combine(directory, $"{shortcutName}.lnk");
        WshShell shell = new();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
        shortcut.TargetPath = targetPath;
        shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
        shortcut.WindowStyle = 1;
        shortcut.Arguments = arguments;
        shortcut.Description = description;
        shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
        shortcut.Save();
    }

    public static void CreateShortcutOnDesktop(string shortcutName, string targetPath, string arguments = null!, string description = null!, string iconLocation = null!)
    {
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        CreateShortcut(desktop, shortcutName, targetPath, arguments, description, iconLocation);
    }
}
