using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Controls;
using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Launch;
using GenshinSwitch.Fetch.Lazy;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Contacts;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Xunkong.Hoyolab;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.DailyNote;
using Xunkong.Hoyolab.GameRecord;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;
using static GenshinSwitch.ViewModels.SettingsViewModel;

namespace GenshinSwitch.ViewModels.Contacts;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "<Pending>")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:Code analysis suppression should have justification", Justification = "<Pending>")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1027:Use tabs correctly", Justification = "<Pending>")]
[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1134:Attributes should not share line", Justification = "<Pending>")]
[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1137:Elements should have the same indentation", Justification = "<Pending>")]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
[ObservableObject]
public partial class ContactViewModel
{
    private readonly HoyolabClient client = new();

    [ObservableProperty]
    private DailyNoteInfoViewModel? dailyNoteViewModel = new();

    [ObservableProperty]
    private SignInInfoViewModel? signInViewModel = new();

    [ObservableProperty]
    private LazyInfoViewModel? lazyInfoViewModel = new();

    [ObservableProperty]
    private SpiralAbyssInfoViewModel? spiralAbyssInfoViewModel = new();

    public ContactProgress ResinInfo { get; set; } = new();
    public ContactProgress SignInInfo { get; set; } = new();
    public ContactProgress FinishedTaskInfo { get; set; } = new();
    public ContactProgress LazyInfo { get; set; } = new();
    public ContactProgress ResinDiscountInfo { get; set; } = new();
    public ContactProgress TransformerInfo { get; set; } = new();
    public ContactProgress ExpeditionInfo { get; set; } = new();
    public ContactProgress HomeCoinInfo { get; set; } = new();
    public ContactProgress SpiralAbyssInfo { get; set; } = new();
    public ContactProgress GcgInfo { get; set; } = new();
    public ContactProgress LInfo { get; set; } = new();

    private bool isRunning = false;
    public bool IsRunning
    {
        get => isRunning;
        set => SetProperty(ref isRunning, value);
    }

    private bool isFetched = false;
    public bool IsFetched
    {
        get => isFetched;
        set => SetProperty(ref isFetched, value);
    }

    public Contact Contact { get; }

    [ObservableProperty]
    private int hintQuestRandomProceIcon = Settings.HintQuestRandomProceIcon;

    public ContactViewModel(Contact contact)
    {
        Contact = contact;

        ResinInfo.IsShown = Settings.HintResinEnable;
        SignInInfo.IsShown = Settings.HintHoyolabEnable;
        FinishedTaskInfo.IsShown = Settings.HintQuestEventsProceEnable;
        ResinDiscountInfo.IsShown = Settings.HintResinDiscountEnable;
        TransformerInfo.IsShown = Settings.HintTransformerEnable;
        ExpeditionInfo.IsShown = Settings.HintInteeExploreEnable;
        HomeCoinInfo.IsShown = Settings.HintHomeCoinEnable;
        SpiralAbyssInfo.IsShown = Settings.HintAbyssEnable;
        LazyInfo.IsShown = Settings.HintQuestRandomProceEnable;
    }

    [RelayCommand]
    private async Task LaunchHoyolabAsync(string type)
    {
        if (type == "android")
        {
            if (RuntimeHelper.IsWin11)
            {
                await HyperionCtrl.LaunchAsync();
            }
            else
            {
                NoticeService.AddNotice("Windows Subsystem for Android™️ NOT installed", "Hoyolab application requested OS higher than Windows 11.");
            }
        }
        else
        {
            await HyperionCtrl.LaunchAsync(HyperionCtrl.Web);
        }
    }

    [RelayCommand]
    private async Task LaunchLazyAsync(string type)
    {
        if (type == "folder")
        {
            FileOpenPicker dialog = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
            };
            dialog.FileTypeFilter.Add(".exe");

            InitializeWithWindow.Initialize(dialog, App.MainWindow.Hwnd);
            StorageFile file = await dialog.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    if (await LazyVerification.VerifyAssembly(file.Path))
                    {
                        Settings.ComponentLazyPath.Set(file.Path);
                        SettingsManager.Save();
                        LazyInfoViewModel!.IsUnlocked = true;
                        Bubble.Success("组件添加成功");
                    }
                    else
                    {
                        Bubble.Warning("请选择正确的组件");
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
        else if (type == "web")
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = "https://github.com/genshin-matrix/genshin-lazy",
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        else if (type == "launch")
        {
            if (string.IsNullOrEmpty(Settings.ComponentLazyPath.Get()))
            {
                if (await LazyProtocol.IsVaildProtocolAsync())
                {
                    await LazyProtocol.LaunchAsync();
                    return;
                }
            }

            if (!File.Exists(Settings.ComponentLazyPath.Get()))
            {
                NoticeService.AddNotice("组件文件不存在", $"{Settings.ComponentLazyPath.Get()}");
                return;
            }

            try
            {
                await Task.Run(async () =>
                {
                    await LazyLauncher.LaunchAsync(Settings.ComponentLazyPath.Get());
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        if (roleFetched == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        try
        {
            bool isSigned = await client.SignInAsync(roleFetched!);

            Logger.Info($"[SignIn] Result={isSigned}");
            if (!isSigned)
            {
                NoticeService.AddNotice("米游社签到", $"账号 {roleFetched!.Nickname} 已签到。");
            }
        }
        catch (Exception e)
        {
            NoticeService.AddNotice("米游社签到", e.Message);
            Logger.Error(e);
        }
    }

    [RelayCommand]
    private void CopyUid()
    {
        string uid = roleFetched?.Uid.ToString();

        if (!string.IsNullOrWhiteSpace(uid))
        {
            ClipboardHelper.SetText(uid);
            Bubble.Information($"角色UID:{uid}已复制到剪贴板");
        }
    }

    [RelayCommand]
    private void CopyCookie()
    {
        string cookie = Contact?.Cookie;

        if (!string.IsNullOrWhiteSpace(cookie))
        {
            ClipboardHelper.SetText(cookie);
            Bubble.Information($"Cookie 已复制到剪贴板");
        }
    }

    [RelayCommand]
    private void CancelRed(string icon)
    {
        switch (icon)
        {
            case "Resin":
                ResinInfo.CancelRed();
                break;
            case "SignIn":
                SignInInfo.CancelRed();
                break;
            case "FinishedTask":
                FinishedTaskInfo.CancelRed();
                break;
            case "Lazy":
                LazyInfo.CancelRed();
                break;
            case "ResinDiscount":
                ResinDiscountInfo.CancelRed();
                break;
            case "Transformer":
                TransformerInfo.CancelRed();
                break;
           case "Expedition":
                ExpeditionInfo.CancelRed();
                break;
            case "HomeCoin":
                HomeCoinInfo.CancelRed();
                break;
            case "SpiralAbyss":
                SpiralAbyssInfo.CancelRed();
                break;
            case "Gcg":
                GcgInfo.CancelRed();
                break;
            case "L":
                LInfo.CancelRed();
                break;
        }
    }

    [ObservableProperty]
    private HoyolabUserInfo? userInfoFetched = new();

    [ObservableProperty]
    private GenshinRoleInfo? roleFetched = new();

    [ObservableProperty]
    private SignInInfo? signInInfoFetched = new();

    [ObservableProperty]
    private GameRecordSummary? gameRecordSummaryFetched = new();

    [ObservableProperty]
    private DailyNoteInfo? dailyNoteFetched = new();

    [ObservableProperty]
    private TravelNotesSummary? travelNotesSummaryFetched = new();

    [ObservableProperty]
    private SpiralAbyssInfo? spiralAbyssInfoFetched = new();

    public async Task FetchAllAsync()
    {
        if (!string.IsNullOrEmpty(Contact.Cookie))
        {
            try
            {
                FetchLazyInfoAsync().Forget();
                await FetchWeekendAsync();
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

    [Obsolete]
    public async Task FetchAppVersionAsync()
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
        if (!string.IsNullOrEmpty(Contact.Cookie))
        {
            try
            {
                userInfoFetched = await client.GetHoyolabUserInfoAsync(Contact.Cookie);

                Logger.Info($"[HoyolabUserInfo] Uid=\"{userInfoFetched.Uid}\" NickName=\"{userInfoFetched.Nickname}\" AvatarUrl=\"{userInfoFetched.AvatarUrl}\"");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchGenshinRoleInfosAsync()
    {
        if (!string.IsNullOrEmpty(Contact.Cookie))
        {
            try
            {
                roleFetched = (await client.GetGenshinRoleInfosAsync(Contact.Cookie)).FirstOrDefault();

                Logger.Info($"[GenshinRoleInfo] Uid=\"{roleFetched!.Uid}\" NickName=\"{roleFetched!.Nickname}\"");
                Contact.NickName = roleFetched!.Nickname;
                Contact.Uid = roleFetched!.Uid;
                Contact.Level = roleFetched!.Level;
                Contact.RegionName = roleFetched!.RegionName;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchSignInInfoAsync()
    {
        if (!Settings.HintHoyolabSign)
        {
            SignInInfo.IsShown = false;
            return;
        }

        if (roleFetched == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (roleFetched != null)
        {
            try
            {
                signInInfoFetched = await client.GetSignInInfoAsync(roleFetched!);

                Logger.Info($"[SignInInfoAsync] IsSign=\"{signInInfoFetched!.IsSign}\"");
                SignInInfo.SetGreen(signInInfoFetched!.IsSign, Settings.HintHoyolabRed);

                SignInViewModel!.IsFetched = true;
                ContactMapperProvider.Map(signInInfoFetched, SignInViewModel);
                OnPropertyChanged(nameof(SignInViewModel));

                if (!signInInfoFetched.IsSign && !SignInInfo.IsNotified && !Settings.HintSilentMode)
                {
                    if (UpdateTime.TodayOffset(10).TotalHours <= 0d)
                    {
                        SignInInfo.IsNotified = true;
                        NoticeService.AddNotice("米游社签到提醒", $"账号 {roleFetched!.Nickname} 未签到，客官请尽快签到吧。");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                SignInInfo.SetYellow(true, Settings.HintHoyolabRed);
                SignInViewModel!.IsFetched = false;
                OnPropertyChanged(nameof(SignInViewModel));
            }
        }
    }

    public async Task FetchGameRecordSummaryAsync()
    {
        if (roleFetched == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (roleFetched != null)
        {
            try
            {
                gameRecordSummaryFetched = await client.GetGameRecordSummaryAsync(roleFetched!);

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
        if (roleFetched == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (roleFetched != null)
        {
            try
            {
                dailyNoteFetched = await client.GetDailyNoteAsync(roleFetched!);

                if (Settings.HintResinEnable)
                {
                    Logger.Info($"[DailyNote] CurrentResin=\"{dailyNoteFetched!.CurrentResin}\"");
                    ResinInfo.ValueMax = dailyNoteFetched!.MaxResin;
                    ResinInfo.Value = dailyNoteFetched!.CurrentResin;
                }
                else
                {
                    ResinInfo.IsShown = false;
                }

                if (Settings.HintQuestEventsProceEnable)
                {
                    Logger.Info($"[DailyNote] FinishedTaskNumber=\"{dailyNoteFetched!.FinishedTaskNumber}\" IsExtraTaskRewardReceived=\"{dailyNoteFetched!.IsExtraTaskRewardReceived}\"");
                    FinishedTaskInfo.ValueMax = dailyNoteFetched!.TotalTaskNumber;
                    FinishedTaskInfo.Value = dailyNoteFetched.FinishedTaskNumber;
                    FinishedTaskInfo.SetGreen(dailyNoteFetched.IsExtraTaskRewardReceived, Settings.HintQuestEventsProceRed);
                }
                else
                {
                    FinishedTaskInfo.IsShown = false;
                }

                if (Settings.HintResinDiscountEnable)
                {
                    ResinDiscountInfo.ValueMax = dailyNoteFetched!.ResinDiscountLimitedNumber;
                    ResinDiscountInfo.Value = dailyNoteFetched!.RemainResinDiscountNumber;

                    int limitDay = UpdateTime.GetSundayDateOffset(4).Days;

                    ResinDiscountInfo.SetGreen(dailyNoteFetched!.RemainResinDiscountNumber == 0, Settings.HintResinDiscountRed && (limitDay < Settings.HintAbyssDeadline));
                }
                else
                {
                    ResinDiscountInfo.IsShown = false;
                }


                if (Settings.HintTransformerEnable)
                {
                    if (dailyNoteFetched.Transformer!.Obtained)
                    {
                        TransformerInfo.SetGreen(!dailyNoteFetched.Transformer.RecoveryTime!.Reached, Settings.HintTransformerRed);
                    }
                    else
                    {
                        TransformerInfo.SetYellow(true, Settings.HintTransformerRed);
                    }
                }
                else
                {
                    TransformerInfo.IsShown = false;
                }

                if (Settings.HintInteeExploreEnable)
                {
                    ExpeditionInfo.Value = dailyNoteFetched!.CurrentExpeditionNumber;
                    ExpeditionInfo.ValueMax = dailyNoteFetched!.MaxExpeditionNumber;

                    if (Settings.HintInteeExploreType == (int)InteeExploreType.Any)
                    {
                        ExpeditionInfo.SetGreen(dailyNoteFetched!.FinishedExpeditionNumber <= 0, Settings.HintInteeExploreRed);
                    }
                    else
                    {
                        ExpeditionInfo.SetGreen(dailyNoteFetched!.FinishedExpeditionNumber < dailyNoteFetched!.MaxExpeditionNumber, Settings.HintInteeExploreRed);
                    }
                }
                else
                {
                    ExpeditionInfo.IsShown = false;
                }

                if (Settings.HintHomeCoinEnable)
                {
                    HomeCoinInfo.Value = dailyNoteFetched!.CurrentHomeCoin;
                    HomeCoinInfo.ValueMax = dailyNoteFetched!.MaxHomeCoin;

                    HomeCoinInfo.SetGreen(HomeCoinInfo.Value.ValueInt32 < Settings.HintHomeCoinLimit, Settings.HintHomeCoinRed);
                }
                else
                {
                    HomeCoinInfo.IsShown = false;
                }

                ContactMapperProvider.Map(dailyNoteFetched, DailyNoteViewModel);
                OnPropertyChanged(nameof(DailyNoteViewModel));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    public async Task FetchSpiralAbyssInfoAsync()
    {
        if (!Settings.HintAbyssEnable)
        {
            SpiralAbyssInfo.IsShown = false;
            return;
        }

        if (roleFetched == null)
        {
            await FetchGenshinRoleInfosAsync();
        }

        if (roleFetched != null)
        {
            try
            {
                spiralAbyssInfoFetched = await client.GetSpiralAbyssInfoAsync(roleFetched!, 1);

                Logger.Info($"[SpiralAbyssInfo] Floors=\"{spiralAbyssInfoFetched!.TotalStar}\"");

                if (spiralAbyssInfoFetched.IsUnlock)
                {
                    SpiralAbyssInfo.ValueMax = 36;
                    SpiralAbyssInfo.Value = spiralAbyssInfoFetched!.TotalStar;

                    int limitDay = (spiralAbyssInfoFetched.EndTime - DateTime.Now).Days;

                    SpiralAbyssInfo.SetGreen(SpiralAbyssInfo.Value >= SpiralAbyssInfo.ValueMax, Settings.HintAbyssRed && (limitDay < Settings.HintAbyssDeadline));
                }
                else
                {
                    SpiralAbyssInfo.SetYellow(true, Settings.HintAbyssRed);
                }

                ContactMapperProvider.Map(spiralAbyssInfoFetched, SpiralAbyssInfoViewModel);
                OnPropertyChanged(nameof(SpiralAbyssInfoViewModel));
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
            if (!Settings.HintQuestRandomProceEnable)
            {
                LazyInfo.IsShown = false;
                return;
            }

            LazyInfoViewModel!.IsUnlocked = await LazyVerification.VerifyAssembly(Settings.ComponentLazyPath.Get());

            if (!LazyInfoViewModel.IsUnlocked)
            {
                LazyInfoViewModel.IsUnlocked = await LazyProtocol.IsVaildProtocolAsync();
            }

            if (Contact.Uid == null)
            {
                await FetchGenshinRoleInfosAsync();
            }

            bool hasLazyToday = await LazyOutputHelper.Check(Contact.Uid?.ToString()!);

            LazyInfo.SetGreen(hasLazyToday, Settings.HintQuestRandomProceRed);

            LazyInfoViewModel!.IsFinished = hasLazyToday;
            LazyInfoViewModel!.IsFetched = true;
            OnPropertyChanged(nameof(LazyInfoViewModel));
        }
        catch (Exception e)
        {
            Logger.Error(e);
            LazyInfo.SetYellow(true, Settings.HintQuestRandomProceRed);
            LazyInfoViewModel!.IsFinished = false;
            LazyInfoViewModel!.IsFetched = false;
            OnPropertyChanged(nameof(LazyInfoViewModel));
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
