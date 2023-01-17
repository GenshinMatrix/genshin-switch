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
using GenshinSwitch.Views;
using MediaInfoLib;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Xunkong.Hoyolab;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.DailyNote;
using Xunkong.Hoyolab.GameRecord;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;

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

    public ContactViewModel(Contact contact)
    {
        Contact = contact;
    }

    [RelayCommand]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "<Pending>")]
    private async void LaunchHoyolab(string type)
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
    [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "<Pending>")]
    private async Task LaunchLazy(string type)
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
                        Bubble.Success($"组件添加成功");
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
                await LaunchLazy("folder");

                if (string.IsNullOrEmpty(Settings.ComponentLazyPath.Get()))
                {
                    return;
                }
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

    [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "<Pending>")]
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
                SignInInfo.IsGreen = signInInfoFetched!.IsSign;
                SignInInfo.IsRed = !SignInInfo.IsGreen;

                ContactMapperProvider.Map(signInInfoFetched, SignInViewModel);
                SignInViewModel!.IsFetched = true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                SignInInfo.IsYellow = true;
                SignInInfo.IsGreen = !SignInInfo.IsYellow;
                SignInInfo.IsRed = !SignInInfo.IsYellow;
                SignInViewModel!.IsFetched = false;
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

                Logger.Info($"[DailyNote] CurrentResin=\"{dailyNoteFetched!.CurrentResin}\"");
                ResinInfo.ValueMax = dailyNoteFetched!.MaxResin;
                ResinInfo.Value = dailyNoteFetched!.CurrentResin;

                Logger.Info($"[DailyNote] FinishedTaskNumber=\"{dailyNoteFetched!.FinishedTaskNumber}\" IsExtraTaskRewardReceived=\"{dailyNoteFetched!.IsExtraTaskRewardReceived}\"");
                FinishedTaskInfo.ValueMax = dailyNoteFetched!.TotalTaskNumber;
                FinishedTaskInfo.Value = dailyNoteFetched.FinishedTaskNumber;
                FinishedTaskInfo.IsGreen = dailyNoteFetched.IsExtraTaskRewardReceived;
                FinishedTaskInfo.IsRed = !FinishedTaskInfo.IsGreen;

                ResinDiscountInfo.ValueMax = dailyNoteFetched!.ResinDiscountLimitedNumber;
                ResinDiscountInfo.Value = dailyNoteFetched!.RemainResinDiscountNumber;
                ResinDiscountInfo.IsGreen = dailyNoteFetched!.RemainResinDiscountNumber == 0;
                ResinDiscountInfo.IsRed = !ResinDiscountInfo.IsGreen;

                if (dailyNoteFetched.Transformer!.Obtained)
                {
                    TransformerInfo.IsYellow = false;
                    TransformerInfo.IsGreen = !dailyNoteFetched.Transformer.RecoveryTime!.Reached;
                    TransformerInfo.IsRed = !TransformerInfo.IsGreen;
                }
                else
                {
                    TransformerInfo.IsYellow = true;
                    TransformerInfo.IsGreen = false;
                    TransformerInfo.IsRed = false;
                }

                ExpeditionInfo.Value = dailyNoteFetched!.CurrentExpeditionNumber;
                ExpeditionInfo.ValueMax = dailyNoteFetched!.MaxExpeditionNumber;
                ExpeditionInfo.IsGreen = dailyNoteFetched!.FinishedExpeditionNumber <= 0;
                ExpeditionInfo.IsRed = !ExpeditionInfo.IsGreen;

                HomeCoinInfo.Value = dailyNoteFetched!.CurrentHomeCoin;
                HomeCoinInfo.ValueMax = dailyNoteFetched!.MaxHomeCoin;
                HomeCoinInfo.IsGreen = (HomeCoinInfo.Value.ValueInt32 / HomeCoinInfo!.ValueMax.ValueInt32) < 0.8d;
                HomeCoinInfo.IsRed = !HomeCoinInfo.IsGreen;

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
            LazyInfoViewModel!.IsUnlocked = await LazyVerification.VerifyAssembly(Settings.ComponentLazyPath.Get());

            if (Contact.Uid == null)
            {
                await FetchGenshinRoleInfosAsync();
            }

            bool hasLazyToday = await Task.Run(() => LazyOutputHelper.Check(Contact.Uid?.ToString()!));

            LazyInfo.IsGreen = hasLazyToday;
            LazyInfo.IsRed = !LazyInfo.IsGreen;

            LazyInfoViewModel!.IsFinished = hasLazyToday;
            LazyInfoViewModel!.IsFetched = true;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            LazyInfoViewModel!.IsFinished = false;
            LazyInfoViewModel!.IsFetched = false;
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
