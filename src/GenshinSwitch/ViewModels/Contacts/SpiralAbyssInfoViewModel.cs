using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Xunkong.Hoyolab.SpiralAbyss;

namespace GenshinSwitch.ViewModels.Contacts;

#pragma warning disable MVVMTK0033

public partial class SpiralAbyssInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int uid;

    [ObservableProperty]
    private int scheduleId;

    [ObservableProperty]
    private DateTimeOffset startTime;

    [ObservableProperty]
    private DateTimeOffset endTime;

    [ObservableProperty]
    private int totalBattleCount;

    [ObservableProperty]
    private int totalWinCount;

    [ObservableProperty]
    private string? maxFloor;

    /// <summary>
    /// 出战最多
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? revealRank = new();

    /// <summary>
    /// 击破最多
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? defeatRank = new();

    /// <summary>
    /// 伤害最高
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? damageRank = new();

    /// <summary>
    /// 承伤最高
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? takeDamageRank = new();

    /// <summary>
    /// 元素战技最多
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? normalSkillRank = new();

    /// <summary>
    /// 元素爆发最多
    /// </summary>
    [ObservableProperty]
    private List<SpiralAbyssRankViewModel>? energySkillRank = new();

    [ObservableProperty]
    private List<SpiralAbyssFloor>? floors = new();

    [ObservableProperty]
    private int totalStar;

    [ObservableProperty]
    private bool isUnlock;
}

/// <summary>
/// 深境螺旋最值统计
/// </summary>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class SpiralAbyssRankViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int avatarId;

    [ObservableProperty]
    private string? avatarIcon;

    [ObservableProperty]
    private int value;

    [ObservableProperty]
    private int rarity;
}

/// <summary>
/// 深境螺旋层
/// </summary>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class SpiralAbyssFloorViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private string? icon;

    [ObservableProperty]
    private bool isUnlock;

    [ObservableProperty]
    private string? settleTime;

    [ObservableProperty]
    private int star;

    [ObservableProperty]
    private int maxStar;

    [ObservableProperty]
    private List<SpiralAbyssLevelViewModel>? levels = new();
}

/// <summary>
/// 深境螺旋间
/// </summary>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class SpiralAbyssLevelViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private int star;

    [ObservableProperty]
    private int maxStar;

    [ObservableProperty]
    private List<SpiralAbyssBattleViewModel>? battles = new();
}

/// <summary>
/// 深境螺旋一场战斗
/// </summary>
[ObservableObject]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Pending>")]
public partial class SpiralAbyssBattleViewModel
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private DateTimeOffset time;

    [ObservableProperty]
    private List<SpiralAbyssAvatar>? avatars = new();
}
