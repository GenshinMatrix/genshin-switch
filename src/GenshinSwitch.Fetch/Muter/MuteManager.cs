using GenshinSwitch.Core;
using GenshinSwitch.Fetch.Launch;
using GenshinSwitch.Fetch.Regedit;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using MMDevices = NAudio.CoreAudioApi.MMDeviceEnumerator;

namespace GenshinSwitch.Fetch.Muter;

public class MuteManager
{
    private static bool autoMute = false;
    public static bool AutoMute
    {
        get => autoMute;
        set
        {
            autoMute = value;
            _ = MuteGameAsync(value);
        }
    }

    static MuteManager()
    {
        ForegroundWindowHelper.Initialize();
        ForegroundWindowHelper.ForegroundWindowChanged += OnForegroundWindowChanged;
    }

    private static async void OnForegroundWindowChanged(ForegroundWindowHelperEventArgs e)
    {
        if (!AutoMute)
        {
            return;
        }

        bool matchProcess = false;
        if (e.Hwnd != IntPtr.Zero)
        {
            if (e.WindowTitle == RegeditKeys.CN || e.WindowTitle == RegeditKeys.OVERSEA)
            {
                matchProcess = true;
            }
            else
            {
#if LEGACY // UAC requested
                if (LaunchCtrl.TryGetProcess(out Process? process))
                {
                    if (process?.Handle == e.Hwnd)
                    {
                        matchProcess = true;
                    }
                };
#endif
            }
        }
        await MuteGameAsync(!matchProcess);
    }

    public static async Task MuteGameAsync(bool isMuted)
    {
        if (LaunchCtrl.TryGetProcess(out Process? process))
        {
            await MuteProcessAsync(process!.Id, isMuted);
        };
    }

    private static async Task MuteProcessAsync(int pid, bool isMuted)
    {
        try
        {
            await Task.Run(() =>
            {
                MuteProcess(pid, isMuted);
            });
        }
        catch (Exception e)
        {
            Logger.Exception(e);
        }
    }

    private static void MuteProcess(int pid, bool isMuted)
    {
        MMDevices audio = new();
        foreach (MMDevice device in audio.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        {
            for (int i = default; i < device.AudioSessionManager.Sessions.Count; i++)
            {
                AudioSessionControl session = device.AudioSessionManager.Sessions[i];

                if (session.GetProcessID == pid)
                {
                    session.SimpleAudioVolume.Mute = isMuted;
                    break;
                }
            }
        }
    }
}
