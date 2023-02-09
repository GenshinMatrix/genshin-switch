using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Controls;
using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Services;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Lazy;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using GenshinSwitch.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.System;

namespace GenshinSwitch.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService themeSelectorService;

    public string AppName => Pack.AppName;
    public string AppVersion => Pack.AppVersion;
    public string UserDataPath => SpecialPathService.Provider.GetPath(string.Empty);

    [ObservableProperty]
    private ElementTheme elementTheme;

    [ObservableProperty]
    private bool autoStart = AutoStartManager.IsEnabled();
    partial void OnAutoStartChanged(bool value)
    {
        AutoStartManager.SetEnabled(value);
    }

    [ObservableProperty]
    private string versionDescription;

    [ObservableProperty]
    private int selectedThemeIndex = (int)Settings.Theme.Get();
    partial void OnSelectedThemeIndexChanged(int value)
    {
        async Task OnSelectedThemeIndexChangedAsync()
        {
            await SwitchThemeAsync((ElementTheme)value);
        }
        _ = OnSelectedThemeIndexChangedAsync();
        Settings.Theme.Set((ElementTheme)value);
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(new ThemeChangedMessage() { Theme = (ElementTheme)value });
    }

    [ObservableProperty]
    private int windowBackdropIndex = Settings.Backdrop.Get() switch
    {
        "None" => 0,
        "Acrylic" => 1,
        "Mica" or _ => 2,
    };
    partial void OnWindowBackdropIndexChanged(int value)
    {
        Settings.Backdrop.Set(value switch
        {
            0 => "None",
            1 => "Acrylic",
            2 or _=> "Mica",
        });
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(new ThemeChangedMessage() { Backdrop = Settings.Backdrop.Get() });
    }

    [ObservableProperty]
    private bool alwaysActiveBackdrop = false;
    partial void OnAlwaysActiveBackdropChanged(bool value)
    {
        throw new NotImplementedException();
    }

    [ObservableProperty]
    private bool hintSilentMode = Settings.HintSilentMode;
    partial void OnHintSilentModeChanged(bool value)
    {
        Settings.HintSilentMode.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintRefreshEnable = Settings.HintRefreshEnable;
    partial void OnHintRefreshEnableChanged(bool value)
    {
        Settings.HintRefreshEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintRefreshMins = Settings.HintRefreshMins;
    partial void OnHintRefreshMinsChanged(int value)
    {
        Settings.HintRefreshMins.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintResinEnable = Settings.HintResinEnable;
    partial void OnHintResinEnableChanged(bool value)
    {
        Settings.HintResinEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintResinLimit = Settings.HintResinLimit;
    partial void OnHintResinLimitChanged(int value)
    {
        Settings.HintResinLimit.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintHoyolabEnable = Settings.HintHoyolabEnable;
    partial void OnHintHoyolabEnableChanged(bool value)
    {
        Settings.HintHoyolabEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintHoyolabSign = Settings.HintHoyolabSign;
    partial void OnHintHoyolabSignChanged(bool value)
    {
        Settings.HintHoyolabSign.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintHoyolabRed = Settings.HintHoyolabRed;
    partial void OnHintHoyolabRedChanged(bool value)
    {
        Settings.HintHoyolabRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintQuestEventsProceEnable = Settings.HintQuestEventsProceEnable;
    partial void OnHintQuestEventsProceEnableChanged(bool value)
    {
        Settings.HintQuestEventsProceEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintQuestEventsProceRed = Settings.HintQuestEventsProceRed;
    partial void OnHintQuestEventsProceRedChanged(bool value)
    {
        Settings.HintQuestEventsProceRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintQuestRandomProceEnable = Settings.HintQuestRandomProceEnable;
    partial void OnHintQuestRandomProceEnableChanged(bool value)
    {
        Settings.HintQuestRandomProceEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintQuestRandomProceIcon = Settings.HintQuestRandomProceIcon;
    partial void OnHintQuestRandomProceIconChanged(int value)
    {
        Settings.HintQuestRandomProceIcon.Set(value);
        SettingsManager.Save();
    }

    [RelayCommand]
    private async Task ChangeHintQuestRandomProceIconAsync(string iconString)
    {
        await Task.CompletedTask;
        try
        {
            if (int.TryParse(iconString, out int icon))
            {
                HintQuestRandomProceIcon = icon;
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    [ObservableProperty]
    private bool hintQuestRandomProceRed = Settings.HintQuestRandomProceRed;
    partial void OnHintQuestRandomProceRedChanged(bool value)
    {
        Settings.HintQuestRandomProceRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintInteeExploreEnable = Settings.HintInteeExploreEnable;
    partial void OnHintInteeExploreEnableChanged(bool value)
    {
        Settings.HintInteeExploreEnable.Set(value);
        SettingsManager.Save();
    }

    internal enum InteeExploreType
    {
        Any = 0,
        All = 1,
    }
    [ObservableProperty]
    private int hintInteeExploreType = Settings.HintInteeExploreType;
    partial void OnHintInteeExploreTypeChanged(int value)
    {
        Settings.HintInteeExploreType.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintInteeExploreRed = Settings.HintInteeExploreRed;
    partial void OnHintInteeExploreRedChanged(bool value)
    {
        Settings.HintInteeExploreRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintHomeCoinEnable = Settings.HintHomeCoinEnable;
    partial void OnHintHomeCoinEnableChanged(bool value)
    {
        Settings.HintHomeCoinEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintHomeCoinLimit = Settings.HintHomeCoinLimit;
    partial void OnHintHomeCoinLimitChanged(int value)
    {
        Settings.HintHomeCoinLimit.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintHomeCoinRed = Settings.HintHomeCoinRed;
    partial void OnHintHomeCoinRedChanged(bool value)
    {
        Settings.HintHomeCoinRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintTransformerEnable = Settings.HintTransformerEnable;
    partial void OnHintTransformerEnableChanged(bool value)
    {
        Settings.HintTransformerEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintTransformerRed = Settings.HintTransformerRed;
    partial void OnHintTransformerRedChanged(bool value)
    {
        Settings.HintTransformerRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintResinDiscountEnable = Settings.HintResinDiscountEnable;
    partial void OnHintResinDiscountEnableChanged(bool value)
    {
        Settings.HintResinDiscountEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintResinDiscountDeadline = Settings.HintResinDiscountDeadline.Get() switch
    {
        1 => 0,
        2 => 1,
        3 => 2,
        7 or _ => 3,
    };
    partial void OnHintResinDiscountDeadlineChanged(int value)
    {
        Settings.HintResinDiscountDeadline.Set(value switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 or _ => 7,
        });
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintResinDiscountRed = Settings.HintResinDiscountRed;
    partial void OnHintResinDiscountRedChanged(bool value)
    {
        Settings.HintResinDiscountRed.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintAbyssEnable = Settings.HintAbyssEnable;
    partial void OnHintAbyssEnableChanged(bool value)
    {
        Settings.HintAbyssEnable.Set(value);
        SettingsManager.Save();
    }

    [ObservableProperty]
    private int hintAbyssDeadline = Settings.HintAbyssDeadline.Get() switch
    {
        1 => 0,
        2 => 1,
        3 => 2,
        7 or _ => 3,
    };
    partial void OnHintAbyssDeadlineChanged(int value)
    {
        Settings.HintAbyssDeadline.Set(value switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 or _ => 7,
        });
        SettingsManager.Save();
    }

    [ObservableProperty]
    private bool hintAbyssRed = Settings.HintAbyssRed;
    partial void OnHintAbyssRedChanged(bool value)
    {
        Settings.HintAbyssRed.Set(value);
        SettingsManager.Save();
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        this.themeSelectorService = themeSelectorService;
        elementTheme = this.themeSelectorService.Theme;
        versionDescription = GetVersionDescription();
    }

    [RelayCommand]
    private async Task LaunchAtWindowsStartupLinkAsync()
    {
        _ = await Launcher.LaunchUriAsync(new Uri("ms-settings:startupapps"));
    }

    [RelayCommand]
    private void CreateDesktopShortCut()
    {
        try
        {
            ShortcutCreator.CreateShortcutOnDesktop(Pack.AppName, Environment.ProcessPath!);
            NoticeService.AddNotice("创建桌面快捷方式", "操作成功");
        }
        catch (Exception e)
        {
            Logger.Error(e);
            NoticeService.AddNotice("Create ShortCut error", "See detail following", e.ToString());
        }
    }

    [RelayCommand]
    private async Task OpenProgramFolderAsync()
    {
        _ = await Launcher.LaunchUriAsync(new Uri($"file://{new FileInfo(Environment.ProcessPath!).Directory!.FullName}"));
    }

    [RelayCommand]
    private async Task SwitchThemeAsync(ElementTheme param)
    {
        if (ElementTheme != param)
        {
            ElementTheme = param;
            await themeSelectorService.SetThemeAsync(param);
        }
    }

    [RelayCommand]
    private void OpenSpecialFolder()
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "explorer.exe",
                Arguments = $"\"{SpecialPathService.Provider.GetFolder()}\"",
            });
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        try
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/genshin-matrix/genshin-switch/releases"));
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    [RelayCommand]
    private void CopyVersion()
    {
        if (!string.IsNullOrWhiteSpace(AppVersion))
        {
            ClipboardHelper.SetText(AppVersion);
            Bubble.Information($"版本号:{AppVersion}已复制到剪贴板");
        }
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            PackageVersion packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    [ObservableProperty]
    private long testLazyElapsedMilliseconds = -1;

    [ObservableProperty]
    private string testLazyMessage = null!;

    [RelayCommand]
    private async Task TestLazyServerAsync()
    {
        try
        {
            if (await LazyRepository.SetupToken())
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                if (!string.IsNullOrEmpty(await LazyRepository.GetFile()))
                {
                    stopwatch.Stop();
                    NoticeService.AddNotice("测试服务器令牌", TestLazyMessage = $"测试通过：耗时 {TestLazyElapsedMilliseconds = stopwatch.ElapsedMilliseconds} 毫秒");
                }
                else
                {
                    TestLazyElapsedMilliseconds = -1;
                    NoticeService.AddNotice("测试服务器令牌", TestLazyMessage = $"测试失败：请保证路径正确后重试");
                }
            }
            else
            {
                TestLazyElapsedMilliseconds = -1;
                NoticeService.AddNotice("未探测到接入服务器令牌", TestLazyMessage = "请查阅组件使用说明设定好令牌后重试");
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    [RelayCommand]
    private async Task SetLazyTokenAsync()
    {
        _ = await new SetLazyTokenContentDialog()
        {
            XamlRoot = App.MainWindow.XamlRoot,
            RequestedTheme = App.MainWindow.ActualTheme,
        }.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowLazyServerAsync()
    {
        try
        {
            if (await LazyRepository.SetupToken())
            {
                string file = await LazyRepository.GetFile();

                if (!string.IsNullOrEmpty(file))
                {
                    _ = await new ShowLazyContentDialog(file)
                    {
                        XamlRoot = App.MainWindow.XamlRoot,
                        RequestedTheme = App.MainWindow.ActualTheme,
                    }.ShowAsync();
                }
                else
                {
                    NoticeService.AddNotice("查看记录失败", "请保证服务器令牌路径正确后重试");
                }
            }
            else
            {
                NoticeService.AddNotice("未探测到接入服务器令牌", "请查阅组件使用说明设定好令牌后重试");
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    [RelayCommand]
    private async Task ResetAsync()
    {
        if (await new MessageBoxX("是否确定要重置接口提醒设置？", "重置接口提醒设置").ShowAsync() == ContentDialogResult.Secondary)
        {
            Settings.HintSilentMode.Reset();
            HintSilentMode = Settings.HintSilentMode;
            Settings.HintRefreshEnable.Reset();
            HintRefreshEnable = Settings.HintRefreshEnable;
            Settings.HintRefreshMins.Reset();
            HintRefreshMins = Settings.HintRefreshMins;
            Settings.HintResinEnable.Reset();
            HintResinEnable = Settings.HintResinEnable;
            Settings.HintResinLimit.Reset();
            HintResinLimit = Settings.HintResinLimit;
            Settings.HintHoyolabEnable.Reset();
            HintHoyolabEnable = Settings.HintHoyolabEnable;
            Settings.HintHoyolabSign.Reset();
            HintHoyolabSign = Settings.HintHoyolabSign;
            Settings.HintHoyolabRed.Reset();
            HintHoyolabRed = Settings.HintHoyolabRed;
            Settings.HintQuestEventsProceEnable.Reset();
            HintQuestEventsProceEnable = Settings.HintQuestEventsProceEnable;
            Settings.HintQuestEventsProceRed.Reset();
            HintQuestEventsProceRed = Settings.HintQuestEventsProceRed;
            Settings.HintQuestRandomProceEnable.Reset();
            HintQuestRandomProceEnable = Settings.HintQuestRandomProceEnable;
            Settings.HintQuestRandomProceIcon.Reset();
            HintQuestRandomProceIcon = Settings.HintQuestRandomProceIcon;
            Settings.HintQuestRandomProceRed.Reset();
            HintQuestRandomProceRed = Settings.HintQuestRandomProceRed;
            Settings.HintInteeExploreEnable.Reset();
            HintInteeExploreEnable = Settings.HintInteeExploreEnable;
            Settings.HintInteeExploreType.Reset();
            HintInteeExploreType = Settings.HintInteeExploreType;
            Settings.HintInteeExploreRed.Reset();
            HintInteeExploreRed = Settings.HintInteeExploreRed;
            Settings.HintHomeCoinEnable.Reset();
            HintHomeCoinEnable = Settings.HintHomeCoinEnable;
            Settings.HintHomeCoinLimit.Reset();
            HintHomeCoinLimit = Settings.HintHomeCoinLimit;
            Settings.HintHomeCoinRed.Reset();
            HintHomeCoinRed = Settings.HintHomeCoinRed;
            Settings.HintTransformerEnable.Reset();
            HintTransformerEnable = Settings.HintTransformerEnable;
            Settings.HintTransformerRed.Reset();
            HintTransformerRed = Settings.HintTransformerRed;
            Settings.HintResinDiscountEnable.Reset();
            HintResinDiscountEnable = Settings.HintResinDiscountEnable;
            Settings.HintResinDiscountDeadline.Reset();
            HintResinDiscountDeadline = Settings.HintResinDiscountDeadline;
            Settings.HintResinDiscountRed.Reset();
            HintResinDiscountRed = Settings.HintResinDiscountRed;
            Settings.HintAbyssEnable.Reset();
            HintAbyssEnable = Settings.HintAbyssEnable;
            Settings.HintAbyssDeadline.Reset();
            HintAbyssDeadline = Settings.HintAbyssDeadline;
            Settings.HintAbyssRed.Reset();
            HintAbyssRed = Settings.HintAbyssRed;
            SettingsManager.Save();
        }
    }
}
