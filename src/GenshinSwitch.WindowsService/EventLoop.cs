using System;
using System.Threading;

namespace GenshinSwitch.WindowsService;

internal class EventLoop
{
    public void Start()
    {
        while (true)
        {
            Thread.Sleep(999);
        }
    }

    public void Stop()
    {
    }
}
