using CommunityToolkit.Mvvm.ComponentModel;
using GenshinSwitch.Fetch.Lazy;
using System.Collections.ObjectModel;

namespace GenshinSwitch.ViewModels;

public partial class ShowLazyViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<LazyInput> lazys = new();

    public void Reload(string file)
    {
        LazyInput[] lazyIns = LazyInputHelper.AnalysisFile(file);

        foreach (LazyInput lazyIn in lazyIns)
        {
            Lazys.Add(lazyIn);
        }
    }
}
