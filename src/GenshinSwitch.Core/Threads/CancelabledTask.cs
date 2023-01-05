namespace GenshinSwitch.Core.Threads;

public class CancelabledTask
{
    public bool IsCanceled { get; private set; } = false;

    public async void DelayAsync(int millisecondsDelay)
    {
        _ = await Task.Run(() => SpinWait.SpinUntil(() => IsCanceled, millisecondsDelay));

        if (IsCanceled)
        {
            throw new GenshinSwitchException(new TimeoutException("User Aborted"));
        }
    }
}
