using Windows.ApplicationModel;

namespace GenshinSwitch;

public class Pack
{
    public static string AppName => Package.Current.DisplayName;

    public static string AppVersion
    {
        get
        {
            PackageVersion v = Package.Current.Id.Version;
            return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
        }
    }

    public static readonly string UriIcon = "ms-appx:///Assets/Logos/Favicon.ico";
}
