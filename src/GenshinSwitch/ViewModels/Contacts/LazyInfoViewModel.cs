using CommunityToolkit.Mvvm.ComponentModel;

namespace GenshinSwitch.ViewModels.Contacts;

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
