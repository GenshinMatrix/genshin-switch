using GenshinSwitch.Helpers;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using WinRT;

namespace GenshinSwitch;

internal static class Program
{
    [STAThread]
    internal static void Main()
    {
        RuntimeHelper.EnsureElevated();
        RuntimeHelper.XamlCheckProcessRequirements();
        ComWrappersSupport.InitializeComWrappers();
        Application.Start((p) =>
        {
            DispatcherQueueSynchronizationContext context = new(DispatcherQueue.GetForCurrentThread());
            SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
    }
}
