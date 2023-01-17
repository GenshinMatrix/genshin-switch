using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenshinSwitch.ViewModels.Contacts;

[ObservableObject]
public partial class SignInInfoViewModel
{
    /// <summary>
    /// 累积签到天数
    /// </summary>
    [ObservableProperty]
    private int totalSignDays;

    /// <summary>
    /// 今天是...
    /// </summary>
    [ObservableProperty]
    private DateTime today;

    /// <summary>
    /// 今日是否已签到
    /// </summary>
    [ObservableProperty]
    private bool isSign;

    [ObservableProperty]
    private bool isSub;

    [ObservableProperty]
    private bool firstBind;

    [ObservableProperty]
    private bool isFirstDayOfMonth;

    [ObservableProperty]
    private int missedCount;

    [ObservableProperty]
    [NotMapped]
    private bool isFetched = false;
}
