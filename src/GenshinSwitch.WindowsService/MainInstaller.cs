using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace GenshinSwitch.WindowsService;

[RunInstaller(true)]
public class MainInstaller : Installer
{
    private readonly ServiceInstaller? serviceInstaller;
    private readonly ServiceProcessInstaller? processInstaller;

    public MainInstaller()
    {
        serviceInstaller = new ServiceInstaller
        {
            ServiceName = "GenshinSwitch.WindowsService",
            DisplayName = "GenshinSwitch",
            Description = "GenshinSwitch extending boxed APIs Provider",
            StartType = ServiceStartMode.Automatic,
        };

        processInstaller = new ServiceProcessInstaller
        {
            Account = ServiceAccount.LocalSystem,
        };

        Installers.Add(serviceInstaller);
        Installers.Add(processInstaller);
    }

    public override void Install(IDictionary stateSaver)
    {
        base.Install(stateSaver);
    }

    public override void Uninstall(IDictionary savedState)
    {
        base.Uninstall(savedState);
    }
}
