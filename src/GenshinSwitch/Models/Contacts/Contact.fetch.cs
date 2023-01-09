using GenshinSwitch.Core;
using GenshinSwitch.Fetch.Lazy;
using Xunkong.Hoyolab;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.DailyNote;
using Xunkong.Hoyolab.GameRecord;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:Code analysis suppression should have justification", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1027:Use tabs correctly", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1134:Attributes should not share line", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1137:Elements should have the same indentation", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public partial class Contact
{
    [YamlIgnore] public HoyolabUserInfo? _UserInfo { get; set; }
    [YamlIgnore] public GenshinRoleInfo? _Role { get; set; }
	[YamlIgnore] public SignInInfo? _SignInInfo { get; set; }
    [YamlIgnore] public GameRecordSummary? _GameRecordSummary { get; set; }
    [YamlIgnore] public DailyNoteInfo? _DailyNote { get; set; }
    [YamlIgnore] public TravelNotesSummary? _TravelNotesSummary { get; set; }
	[YamlIgnore] public string? _SpiralAbyss { get; set; }
	[YamlIgnore] public SpiralAbyssInfo? _SpiralAbyssInfo { get; set; }

    private readonly HoyolabClient client = new();

    public async Task FetchAllAsync()
    {
        if (!string.IsNullOrEmpty(Cookie))
        {
            try
            {
                await FetchLazyInfoAsync();
                await FetchWeekendAsync();
                await FetchAppVersion();
                await FetchHoyolabUserInfoAsync();
                await FetchGenshinRoleInfosAsync();
                await FetchSignInInfoAsync();
                await FetchDailyNoteAsync();
                await FetchSpiralAbyssInfoAsync();

                IsFetched = true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
	}

    public async Task FetchAppVersion()
    {
        try
        {
            await client.FetchAppVersion();
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    public async Task FetchHoyolabUserInfoAsync()
    {
        if (!string.IsNullOrEmpty(Cookie))
        {
            try
            {
                _UserInfo = await client.GetHoyolabUserInfoAsync(Cookie);

                Logger.Info($"[HoyolabUserInfo] Uid=\"{_UserInfo.Uid}\" NickName=\"{_UserInfo.Nickname}\" AvatarUrl=\"{_UserInfo.AvatarUrl}\"");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchGenshinRoleInfosAsync()
    {
        if (!string.IsNullOrEmpty(Cookie))
        {
            try
            {
                _Role = (await client.GetGenshinRoleInfosAsync(Cookie)).FirstOrDefault();

                Logger.Info($"[GenshinRoleInfo] Uid=\"{_Role!.Uid}\" NickName=\"{_Role!.Nickname}\"");
                NickName = _Role!.Nickname;
                Uid = _Role!.Uid;
                Level = _Role!.Level;
                RegionName = _Role!.RegionName;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchSignInInfoAsync()
    {
        if (_Role == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (_Role != null)
        {
            try
            {
                _SignInInfo = await client.GetSignInInfoAsync(_Role!);

                Logger.Info($"[SignInInfoAsync] IsSign=\"{_SignInInfo!.IsSign}\"");
                SignInInfo.IsGreen = _SignInInfo!.IsSign;
                SignInInfo.IsRed = !SignInInfo.IsGreen;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchGameRecordSummaryAsync()
    {
        if (_Role == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (_Role != null)
        {
            try
            {
                _GameRecordSummary = await client.GetGameRecordSummaryAsync(_Role!);

                Logger.Info($"[GameRecordSummary] fetched");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchDailyNoteAsync()
    {
        if (_Role == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (_Role != null)
        {
            try
            {
                _DailyNote = await client.GetDailyNoteAsync(_Role!);

                Logger.Info($"[DailyNote] CurrentResin=\"{_DailyNote!.CurrentResin}\"");
                ResinInfo.ValueMax = _DailyNote!.MaxResin;
                ResinInfo.Value = _DailyNote!.CurrentResin;

                Logger.Info($"[DailyNote] FinishedTaskNumber=\"{_DailyNote!.FinishedTaskNumber}\" IsExtraTaskRewardReceived=\"{_DailyNote!.IsExtraTaskRewardReceived}\"");
                FinishedTaskInfo.ValueMax = _DailyNote!.TotalTaskNumber;
                FinishedTaskInfo.Value = _DailyNote.FinishedTaskNumber;
                FinishedTaskInfo.IsGreen = _DailyNote.IsExtraTaskRewardReceived;
                FinishedTaskInfo.IsRed = !FinishedTaskInfo.IsGreen;

                ResinDiscountInfo.ValueMax = _DailyNote!.ResinDiscountLimitedNumber;
                ResinDiscountInfo.Value = _DailyNote!.RemainResinDiscountNumber;
                ResinDiscountInfo.IsGreen = _DailyNote!.RemainResinDiscountNumber == 0;
                ResinDiscountInfo.IsRed = !ResinDiscountInfo.IsGreen;

                if (_DailyNote.Transformer!.Obtained)
                {
                    TransformerInfo.IsYellow = false;
                    TransformerInfo.IsGreen = !_DailyNote.Transformer.RecoveryTime!.Reached;
                    TransformerInfo.IsRed = !TransformerInfo.IsGreen;
                }
                else
                {
                    TransformerInfo.IsYellow = true;
                    TransformerInfo.IsGreen = false;
                    TransformerInfo.IsRed = false;
                }

                ExpeditionInfo.Value = _DailyNote!.CurrentExpeditionNumber;
                ExpeditionInfo.ValueMax = _DailyNote!.MaxExpeditionNumber;
                ExpeditionInfo.IsGreen = _DailyNote!.FinishedExpeditionNumber <= 0;
                ExpeditionInfo.IsRed = !ExpeditionInfo.IsGreen;

                HomeCoinInfo.Value = _DailyNote!.CurrentHomeCoin;
                HomeCoinInfo.ValueMax = _DailyNote!.MaxHomeCoin;
                HomeCoinInfo.IsGreen = (HomeCoinInfo.Value.ValueInt32 / HomeCoinInfo!.ValueMax.ValueInt32) < 0.8d;
                HomeCoinInfo.IsRed = !HomeCoinInfo.IsGreen;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchSpiralAbyssInfoAsync()
    {
        if (_Role == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (_Role != null)
        {
            try
            {
                _SpiralAbyssInfo = await client.GetSpiralAbyssInfoAsync(_Role!, 1);

                Logger.Info($"[SpiralAbyssInfo] Floors=\"{_SpiralAbyssInfo!.TotalStar}\"");

                if (_SpiralAbyssInfo.IsUnlock)
                {
                    SpiralAbyssInfo.ValueMax = 36;
                    SpiralAbyssInfo.Value = _SpiralAbyssInfo!.TotalStar;
                    SpiralAbyssInfo.IsYellow = false;
                    SpiralAbyssInfo.IsGreen = SpiralAbyssInfo.Value >= SpiralAbyssInfo.ValueMax;
                    SpiralAbyssInfo.IsRed = !SpiralAbyssInfo.IsGreen;
                }
                else
                {
                    SpiralAbyssInfo.IsYellow = true;
                    SpiralAbyssInfo.IsGreen = false;
                    SpiralAbyssInfo.IsRed = false;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchLazyInfoAsync()
    {
        try
        {
            if (Uid == null)
            {
                await FetchGenshinRoleInfosAsync();
            }

            bool hasLazyToday = await Task.Run(() => LazyOutputHelper.Check(Uid?.ToString()!));

            LazyInfo.IsGreen = hasLazyToday;
            LazyInfo.IsRed = !LazyInfo.IsGreen;
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    public async Task FetchWeekendAsync()
    {
        try
        {
            GcgInfo.IsShown = false;
            LInfo.IsShown = false;
            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
