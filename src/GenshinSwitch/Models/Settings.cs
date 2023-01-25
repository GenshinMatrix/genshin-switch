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
    public static SettingsDefinition<ElementTheme> AppBackgroundRequestedTheme { get; } = new(nameof(AppBackgroundRequestedTheme), ElementTheme.Default);
    public static SettingsDefinition<Dictionary<string, Contact>> Contacts { get; } = new(nameof(Contacts), new());
    public static SettingsDefinition<RelaunchMethods> RelaunchMethod { get; } = new(nameof(RelaunchMethod), RelaunchMethods.Kill);
    public static SettingsDefinition<string> ComponentLazyPath { get; } = new(nameof(ComponentLazyPath), string.Empty);

    public static SettingsDefinition<bool> HintResinEnable { get; } = new(nameof(HintResinEnable), true);
    public static SettingsDefinition<int> HintResinLimit { get; } = new(nameof(HintResinLimit), 100);

    public static SettingsDefinition<bool> HintHoyolabEnable { get; } = new(nameof(HintHoyolabEnable), true);
    public static SettingsDefinition<bool> HintHoyolabSign { get; } = new(nameof(HintHoyolabRed), true);
    public static SettingsDefinition<bool> HintHoyolabRed { get; } = new(nameof(HintHoyolabRed), false);


}
