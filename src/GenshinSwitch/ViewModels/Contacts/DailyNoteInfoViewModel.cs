using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Xunkong.Hoyolab.DailyNote;

namespace GenshinSwitch.ViewModels.Contacts;

[ObservableObject]
public partial class DailyNoteInfoViewModel
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int uid;

    [ObservableProperty]
    private string? nickname;

    /// <summary>
    /// 获取实时便笺时的时间
    /// </summary>
    private DateTimeOffset nowTime = DateTimeOffset.Now;
    public DateTimeOffset NowTime
    {
        get => nowTime;
        set
        {
            SetProperty(ref nowTime, value);
            OnPropertyChanged(nameof(ResinFullTime));
        }
    }

    /// <summary>
    /// 当前树脂
    /// </summary>
    [ObservableProperty]
    private int currentResin;

    /// <summary>
    /// 最大树脂
    /// </summary>
    [ObservableProperty]
    private int maxResin;

    /// <summary>
    /// 树脂剩余恢复时间
    /// </summary>
    private TimeSpan resinRecoveryTime;
    public TimeSpan ResinRecoveryTime
    {
        get => resinRecoveryTime;
        set
        {
            SetProperty(ref resinRecoveryTime, value);
            OnPropertyChanged(nameof(IsResinFull));
            OnPropertyChanged(nameof(ResinFullTime));
        }
    }

    /// <summary>
    /// 树脂是否恢复满
    /// </summary>
    public bool IsResinFull => ResinRecoveryTime == TimeSpan.Zero;

    /// <summary>
    /// 树脂恢复满的时刻
    /// </summary>
    public DateTimeOffset ResinFullTime => NowTime + ResinRecoveryTime;

    /// <summary>
    /// 委托完成数
    /// </summary>
    [ObservableProperty]
    private int finishedTaskNumber;

    /// <summary>
    /// 委托总数
    /// </summary>
    [ObservableProperty]
    private int totalTaskNumber;

    /// <summary>
    /// 4次委托额外奖励是否领取
    /// </summary>
    [ObservableProperty]
    private bool isExtraTaskRewardReceived;

    /// <summary>
    /// 剩余周本树脂减半次数
    /// </summary>
    [ObservableProperty]
    private int remainResinDiscountNumber;

    /// <summary>
    /// 周本树脂减半总次数
    /// </summary>
    [ObservableProperty]
    private int resinDiscountLimitedNumber;

    /// <summary>
    /// 当前派遣数
    /// </summary>
    [ObservableProperty]
    private int currentExpeditionNumber;

    /// <summary>
    /// 已完成派遣数
    /// </summary>
    public int FinishedExpeditionNumber => Expeditions?.Count(x => x.IsFinished) ?? 0;

    /// <summary>
    /// 最大派遣数
    /// </summary>
    [ObservableProperty]
    private int maxExpeditionNumber;

    /// <summary>
    /// 探索派遣
    /// </summary>
    private List<Expedition>? expeditions = new();
    public List<Expedition>? Expeditions
    {
        get => expeditions;
        set
        {
            SetProperty(ref expeditions, value);
            OnPropertyChanged(nameof(FinishedExpeditionNumber));
        }
    }

    /// <summary>
    /// 当前洞天宝钱
    /// </summary>
    [ObservableProperty]
    private int currentHomeCoin;

    /// <summary>
    /// 最大洞天宝钱
    /// </summary>
    [ObservableProperty]
    private int maxHomeCoin;

    /// <summary>
    /// 洞天宝钱剩余恢复时间
    /// </summary>
    [ObservableProperty]
    private TimeSpan homeCoinRecoveryTime;

    /// <summary>
    /// 参量质变仪
    /// </summary>
    [ObservableProperty]
    private TransformerViewModel? transformer = new();

    /// <summary>
    /// 洞天宝钱是否已满
    /// </summary>
    public bool IsHomeCoinFull => HomeCoinRecoveryTime == TimeSpan.Zero;

    /// <summary>
    /// 洞天宝钱攒满时刻
    /// </summary>
    public DateTimeOffset HomeCoinFullTime => NowTime + HomeCoinRecoveryTime;
}

/// <summary>
/// 参量质变仪
/// </summary>
[ObservableObject]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class TransformerViewModel
{
    /// <summary>
    /// 是否获得
    /// </summary>
    [ObservableProperty]
    private bool obtained;

    /// <summary>
    /// 剩余时间
    /// </summary>
    [ObservableProperty]
    private TransformerRecoveryTimeViewModel? recoveryTime = new();

    /// <summary>
    /// Wiki url
    /// </summary>
    [ObservableProperty]
    private string? wiki;
}

/// <summary>
/// 参量质变仪恢复时间
/// <para>时间四值中仅有 <see cref="Day"/> 或 <see cref="Hour"/> 有值</para>
/// </summary>
[ObservableObject]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class TransformerRecoveryTimeViewModel
{
    [ObservableProperty]
    private int day;

    [ObservableProperty]
    private int hour;

    [ObservableProperty]
    private int minute;

    [ObservableProperty]
    private int second;

    /// <summary>
    /// 是否可再次使用
    /// </summary>
    [ObservableProperty]
    private bool reached;
}
