using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace GenshinSwitch.ViewModels.Contacts;

[Obfuscation]
public class ContactProgress : ObservableRecipient
{
    /// <summary>
    /// Icon shown base on user settings
    /// </summary>
    private bool isShown = true;
    public bool IsShown
    {
        get => isShown;
        set => SetProperty(ref isShown, value);
    }

    private ContactProgressValue value = 0d;
    public ContactProgressValue Value
    {
        get => value;
        set => SetProperty(ref this.value, value);
    }

    private ContactProgressValue valueMin = 0d;
    public ContactProgressValue ValueMin
    {
        get => valueMin;
        set => SetProperty(ref valueMin, value);
    }

    private ContactProgressValue valueMax = 100d;
    public ContactProgressValue ValueMax
    {
        get => valueMax;
        set => SetProperty(ref valueMax, value);
    }

    /// <summary>
    /// Icon opacity
    /// </summary>
    private double opacity = 1d;
    public double Opacity
    {
        get => opacity;
        set => SetProperty(ref opacity, value);
    }

    private bool isNotified = false;
    public bool IsNotified
    {
        get => isNotified;
        set => SetProperty(ref isNotified, value);
    }

    /// <summary>
    /// Icon with hint
    /// </summary>
    private bool isRed = false;
    public bool IsRed
    {
        get => isRed;
        set => SetProperty(ref isRed, value);
    }

    /// <summary>
    /// Icon with warning
    /// </summary>
    private bool isYellow = false;
    public bool IsYellow
    {
        get => isYellow;
        set => SetProperty(ref isYellow, value);
    }

    /// <summary>
    /// Icon with okay
    /// </summary>
    private bool isGreen = false;
    public bool IsGreen
    {
        get => isGreen;
        set => SetProperty(ref isGreen, value);
    }

    private bool isRedCanceled = false;
    public bool IsRedCanceled
    {
        get => isRedCanceled;
        set => SetProperty(ref isRedCanceled, value);
    }

    public void CancelRed()
    {
        IsRedCanceled = true;
        IsRed = false;
    }

    public void SetGreen(bool value, bool isRedable = true)
    {
        IsGreen = value;
        IsRed = IsRedCanceled ? false : isRedable ? !value : false;
        IsYellow = false;
    }

    public void SetYellow(bool value, bool isRedable = true)
    {
        IsYellow = value;
        IsRed = IsRedCanceled ? false : isRedable ? !value : false;
        IsGreen = !value;
    }

    public void SetRed(bool value, bool isRedable = true)
    {
        IsRed = IsRedCanceled ? false : isRedable ? value : false;
        IsGreen = !value;
        IsYellow = false;
    }
}
