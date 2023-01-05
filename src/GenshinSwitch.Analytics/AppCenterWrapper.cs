using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace YouiToolkit
{
    public static class AppCenterWrapper
    {
        private static string userId = null;
        public static string UserId => userId;

        private static string appSecret = null;
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

        public static void VisitWebsite(string owner = null, string appName = null, bool isOrgs = true)
        {
            try
            {
                if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(appName))
                {
                    _ = Process.Start("https://appcenter.ms");
                }
                else
                {
                    // https://appcenter.ms/orgs/Youibot/apps/YouiToolkit_Debug/crashes/errors
                    // https://appcenter.ms/orgs/Youibot/apps/YouiToolkit_Release/crashes/errors
                    _ = Process.Start($"https://appcenter.ms/{(isOrgs ? "orgs" : "users")}/{owner}/apps/{appName}/crashes/errors");
                }
            }
            catch
            {
            }
        }
    }
}
