using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Core;
using Lnk;

namespace GenshinSwitch.Helpers;

public class AutoStartupHelper
{
    public static string StartupFolder => Environment.GetEnvironmentVariable("windir") + @"\..\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\";

    public void Enable()
    {
        try
        {
            if (Directory.Exists(StartupFolder))
            {
                ShortcutCreator.CreateShortcut(StartupFolder, Pack.AppName, Environment.ProcessPath!);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
            NoticeService.AddNotice("Create Startup ShortCut error", "See detail following", e.ToString());
        }
    }

    public bool IsEnabled()
    {
        try
        {
            if (Directory.Exists(StartupFolder))
            {
                string lnk = StartupFolder + Pack.AppName + ".lnk";
                if (File.Exists(lnk))
                {
                    byte[] raw = File.ReadAllBytes(lnk);
                    if (raw[0] == 0x4c)
                    {
                        LnkFile lnkObj = new LnkFile(raw, lnk);

                        if (lnkObj.LocalPath == Environment.ProcessPath!)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return false;
    }

    public void Disable()
    {
        try
        {
            string lnk = StartupFolder + Pack.AppName + ".lnk";

            if (File.Exists(lnk))
            {
                File.Delete(lnk);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
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
