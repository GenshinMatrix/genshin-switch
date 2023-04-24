using GenshinSwitch.Helpers;
using Windows.ApplicationModel;

namespace GenshinSwitch;

public class Pack
{
    public static string AppName => "GenshinSwitch";

    public static string AppVersion
    {
        get
        {
            if (RuntimeHelper.IsMSIX)
            {
                PackageVersion v = Package.Current.Id.Version;
                return $"v{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
            return AssemblyUtils.GetAssemblyVersion(typeof(App).Assembly, prefix: "v");
        }
    }

    public static readonly string UriIcon = "ms-appx:///Assets/Logos/Favicon.ico";
}
