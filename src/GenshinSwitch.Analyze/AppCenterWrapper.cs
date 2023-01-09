using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Diagnostics;

namespace GenshinSwitch.Analyze;

public static class AppCenterWrapper
{
    private static string userId = null!;
    public static string UserId => userId;

    private static readonly string appSecret = null!;
    public static string AppSecret => appSecret;

    public static bool IsStarted { get; private set; } = false;

    public static void Start(string userId, string appSecret)
    {
        AppCenter.SetUserId(AppCenterWrapper.userId = userId);
#if VERBOSE
        AppCenter.LogLevel = LogLevel.Verbose;
#endif
        AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));
        IsStarted = true;
    }

    public static void TrackError(Exception exception, IDictionary<string, string> properties = null, params ErrorAttachmentLog[] attachments)
    {
        Crashes.TrackError(exception, properties, attachments);
    }

    public static void VisitWebsite(string owner = null!, string appName = null!, bool isOrgs = true)
    {
        try
        {
            if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(appName))
            {
                _ = Process.Start("https://appcenter.ms");
            }
            else
            {
                _ = Process.Start($"https://appcenter.ms/{(isOrgs ? "orgs" : "users")}/{owner}/apps/{appName}/crashes/errors");
            }
        }
        catch
        {
        }
    }
}
