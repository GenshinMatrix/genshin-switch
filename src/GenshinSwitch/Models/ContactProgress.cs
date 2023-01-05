using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace GenshinSwitch.Models;

[Obfuscation]
public class ContactProgress : ObservableRecipient
{
    protected bool isShown = true;
    public bool IsShown
    {
        get => isShown;
        set => SetProperty(ref isShown, value);
    }

    protected double opacity = 1d;
    public double Opacity
    {
        get => opacity;
        set => SetProperty(ref opacity, value);
    }

    protected bool isRed = false;
    public bool IsRed
    {
        get => isRed;
        set => SetProperty(ref isRed, value);
    }

    protected bool isYellow = false;
    public bool IsYellow
    {
        get => isYellow;
        set => SetProperty(ref isYellow, value);
    }
}
