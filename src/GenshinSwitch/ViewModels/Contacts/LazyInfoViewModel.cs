using CommunityToolkit.Mvvm.ComponentModel;

namespace GenshinSwitch.ViewModels.Contacts;

#pragma warning disable MVVMTK0033

[ObservableObject]
public partial class LazyInfoViewModel
{
    [ObservableProperty]
    private bool isUnlocked = false;

    [ObservableProperty]
    private bool isFetched = false;

    [ObservableProperty]
    private bool isFinished = false;
}
