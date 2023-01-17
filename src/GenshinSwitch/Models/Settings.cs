using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Launch;
using System.Reflection;

namespace GenshinSwitch.Models;

[Obfuscation]
public class Settings
{
    public static SettingsDefinition<string> Language { get; } = new(nameof(Language), string.Empty);
    public static SettingsDefinition<string> Backdrop { get; } = new(nameof(Backdrop), string.Empty);
    public static SettingsDefinition<Dictionary<string, Contact>> Contacts { get; } = new(nameof(Contacts), new());
    public static SettingsDefinition<RelaunchMethods> RelaunchMethod { get; } = new(nameof(RelaunchMethod), RelaunchMethods.Kill);
    public static SettingsDefinition<string> ComponentLazyPath { get; } = new(nameof(ComponentLazyPath), string.Empty);
    public static SettingsDefinition<int> ResinHintLimit { get; } = new(nameof(ResinHintLimit), 100);
}
