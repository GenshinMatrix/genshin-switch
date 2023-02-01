using GenshinSwitch.Core;

namespace GenshinSwitch.Helpers;

public static class CancelabledTask
{
    public static bool IsCanceled { get; private set; } = false;

    public static async Task DelayAsync(int millisecondsDelay)
    {
        _ = await Task.Run(() => SpinWait.SpinUntil(() => IsCanceled, millisecondsDelay));

        if (IsCanceled)
        {
            throw new GenshinSwitchException(new TimeoutException("User Aborted"));
        }
    }
}
