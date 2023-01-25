using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace GenshinSwitch.ViewModels;

[ObservableObject]
public partial class SettingsContactProgressViewModel
{
    [ObservableProperty]
    private bool isEnabled;

    [ObservableProperty]
    private double hintValue;

    [ObservableProperty]
    private bool isRed;

    public SettingsContactProgressViewModel(PropertyChangedEventHandler? changedHandler = null!)
    {
        if (changedHandler != null)
        {
            PropertyChanged += changedHandler;
        }
    }
}
