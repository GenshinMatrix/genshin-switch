using CommunityToolkit.WinUI.Notifications;

namespace GenshinSwitch.Controls.Notice;

internal static class NoticeService
{
    static NoticeService()
    {
        ClearNotice();
    }

    public static void AddNotice(string header, string title, string detail = null!, ToastDuration duration = ToastDuration.Short)
    {
        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddAttributionTextIf(!string.IsNullOrEmpty(detail), detail)
            .SetToastDuration(duration)
            .ShowSafe();
    }

    public static void AddNoticeWithButton(string header, string title, string button, (string, string) arg, ToastDuration duration = ToastDuration.Short)
    {
        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddButton(new ToastButton().SetContent(button).AddArgument(arg.Item1, arg.Item2).SetBackgroundActivation())
            .SetToastDuration(duration)
            .ShowSafe();
    }

    public static void ClearNotice()
    {
        try
        {
            ToastNotificationManagerCompat.History.Clear();
        }
        catch
        {
        }
    }
}
