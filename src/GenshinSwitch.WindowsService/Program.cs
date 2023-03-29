using System;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("b2c2ce92-64cc-48fe-9fff-7fcb16ba0e90")]

namespace GenshinSwitch.WindowsService;

internal static class Program
{
    public static void Main(string[] args)
    {
#if false
        if (args.Length > 0)
        {
            if (args[0].Equals("start", StringComparison.OrdinalIgnoreCase)
             || args[0].Equals("/i", StringComparison.OrdinalIgnoreCase))
            {
                MainRegister.Register();
                MainRunner.Start();
                return;
            }
            else if (args[0].Equals("stop", StringComparison.OrdinalIgnoreCase)
                  || args[0].Equals("/u", StringComparison.OrdinalIgnoreCase))
            {
                MainRunner.Stop();
                MainRegister.Unregister();
                return;
            }
            else if (args[0].Equals("serve", StringComparison.OrdinalIgnoreCase)
                  || args[0].Equals("/s", StringComparison.OrdinalIgnoreCase))
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MainService(),
                };
                ServiceBase.Run(ServicesToRun);
                return;
            }
        }
#endif
        RuntimeHelper.CheckSingleInstance();
        RuntimeHelper.EnsureElevated();
        new AutoStartRegistyHelper().Enable();
        new MainService().StartServe();
        new EventLoop().Start();
    }
}
