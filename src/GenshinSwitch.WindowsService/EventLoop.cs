using System.Threading;

namespace GenshinSwitch.WindowsService;

internal class EventLoop
{
    public void Start()
    {
        while (true)
        {
            Thread.Sleep(int.MaxValue);
        }
    }

    public void Stop()
    {
    }
}
