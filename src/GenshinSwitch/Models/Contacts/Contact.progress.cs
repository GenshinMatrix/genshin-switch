using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

public partial class Contact
{
    [YamlIgnore] public ContactProgress ResinInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress SignInInfo { get; set; } = new();

    private bool isRunning = false;
    [YamlIgnore] public bool IsRunning
    {
        get => isRunning;
        set => SetProperty(ref isRunning, value);
    }
}
