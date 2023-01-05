using GenshinSwitch.Fetch.Attributes;
using System.Reflection;

namespace GenshinSwitch.Fetch.Launch;

[Obfuscation]
public enum RelaunchMethods
{
    None,
    Kill,
    [UACRequest]
    Close,
}
