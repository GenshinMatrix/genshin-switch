using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

[assembly: ComVisible(false)]
[assembly: Guid("b2c2ce92-64cc-48fe-9fff-7fcb16ba0e90")]

namespace GenshinSwitch.WindowsService;

internal static class Program
{
    public static void Main(string[] args)
    {
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
        }

        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
            new MainService(),
        };
        ServiceBase.Run(ServicesToRun);
    }
}
