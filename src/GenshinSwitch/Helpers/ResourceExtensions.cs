using Microsoft.Windows.ApplicationModel.Resources;

namespace GenshinSwitch.Helpers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:Static readonly fields should begin with upper-case letter", Justification = "<Pending>")]
public static class ResourceExtensions
{
    private static readonly ResourceLoader resourceLoader = new();

    public static string GetLocalized(this string resourceKey) => resourceLoader.GetString(resourceKey);
}
