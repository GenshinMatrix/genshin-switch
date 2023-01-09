using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

public partial class Contact
{
    [YamlIgnore] public ContactProgress ResinInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress SignInInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress FinishedTaskInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress LazyInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress ResinDiscountInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress TransformerInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress ExpeditionInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress HomeCoinInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress SpiralAbyssInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress GcgInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress LInfo { get; set; } = new();

    private bool isRunning = false;
    [YamlIgnore] public bool IsRunning
    {
        get => isRunning;
        set => SetProperty(ref isRunning, value);
    }

    private bool isFetched = false;
    [YamlIgnore] public bool IsFetched
    {
        get => isFetched;
        set => SetProperty(ref isFetched, value);
    }
}
