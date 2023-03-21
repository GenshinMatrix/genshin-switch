using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Launch;
using Microsoft.UI.Xaml;
using System.Reflection;

namespace GenshinSwitch.Models;

[Obfuscation]
public class Settings
{
    public static SettingsDefinition<string> Language { get; } = new(nameof(Language), string.Empty);
    public static SettingsDefinition<string> Backdrop { get; } = new(nameof(Backdrop), string.Empty);
    public static SettingsDefinition<int> CloseButtonMethod { get; } = new(nameof(CloseButtonMethod), 1);
    public static SettingsDefinition<ElementTheme> Theme { get; } = new(nameof(Theme), ElementTheme.Default);
    public static SettingsDefinition<Dictionary<string, Contact>> Contacts { get; } = new(nameof(Contacts), new());
    public static SettingsDefinition<RelaunchMethods> RelaunchMethod { get; } = new(nameof(RelaunchMethod), RelaunchMethods.Kill);
    public static SettingsDefinition<string> ComponentLazyPath { get; } = new(nameof(ComponentLazyPath), string.Empty);
    public static SettingsDefinition<bool> AutoMute { get; } = new(nameof(AutoMute), false);
    public static SettingsDefinition<bool> AutoCheckRunning { get; } = new(nameof(AutoCheckRunning), false);

    public static SettingsDefinition<bool> HintSilentMode { get; } = new(nameof(HintSilentMode), false);
    public static SettingsDefinition<bool> HintRefreshEnable { get; } = new(nameof(HintRefreshEnable), false);
    public static SettingsDefinition<int> HintRefreshMins { get; } = new(nameof(HintRefreshMins), 15);

    public static SettingsDefinition<bool> HintResinEnable { get; } = new(nameof(HintResinEnable), true);
    public static SettingsDefinition<int> HintResinLimit { get; } = new(nameof(HintResinLimit), 100);

    public static SettingsDefinition<bool> HintHoyolabEnable { get; } = new(nameof(HintHoyolabEnable), true);
    public static SettingsDefinition<bool> HintHoyolabSign { get; } = new(nameof(HintHoyolabSign), true);
    public static SettingsDefinition<bool> HintHoyolabRed { get; } = new(nameof(HintHoyolabRed), false);

    public static SettingsDefinition<bool> HintQuestEventsProceEnable { get; } = new(nameof(HintQuestEventsProceEnable), true);
    public static SettingsDefinition<bool> HintQuestEventsProceRed { get; } = new(nameof(HintQuestEventsProceRed), false);

    public static SettingsDefinition<bool> HintQuestRandomProceEnable { get; } = new(nameof(HintQuestRandomProceEnable), true);
    public static SettingsDefinition<int> HintQuestRandomProceIcon { get; } = new(nameof(HintQuestRandomProceIcon), 0);
    public static SettingsDefinition<bool> HintQuestRandomProceRed { get; } = new(nameof(HintQuestRandomProceRed), false);

    public static SettingsDefinition<bool> HintInteeExploreEnable { get; } = new(nameof(HintInteeExploreEnable), true);
    public static SettingsDefinition<int> HintInteeExploreType { get; } = new(nameof(HintInteeExploreType), 0);
    public static SettingsDefinition<bool> HintInteeExploreRed { get; } = new(nameof(HintInteeExploreRed), true);

    public static SettingsDefinition<bool> HintHomeCoinEnable { get; } = new(nameof(HintHomeCoinEnable), true);
    public static SettingsDefinition<int> HintHomeCoinLimit { get; } = new(nameof(HintHomeCoinLimit), 1800);
    public static SettingsDefinition<bool> HintHomeCoinRed { get; } = new(nameof(HintHomeCoinRed), true);

    public static SettingsDefinition<bool> HintTransformerEnable { get; } = new(nameof(HintTransformerEnable), true);
    public static SettingsDefinition<bool> HintTransformerRed { get; } = new(nameof(HintTransformerRed), true);

    public static SettingsDefinition<bool> HintResinDiscountEnable { get; } = new(nameof(HintResinDiscountEnable), true);
    public static SettingsDefinition<int> HintResinDiscountDeadline { get; } = new(nameof(HintResinDiscountDeadline), 1);
    public static SettingsDefinition<bool> HintResinDiscountRed { get; } = new(nameof(HintResinDiscountRed), true);

    public static SettingsDefinition<bool> HintAbyssEnable { get; } = new(nameof(HintAbyssEnable), true);
    public static SettingsDefinition<int> HintAbyssDeadline { get; } = new(nameof(HintAbyssDeadline), 1);
    public static SettingsDefinition<bool> HintAbyssRed { get; } = new(nameof(HintAbyssRed), true);
}
