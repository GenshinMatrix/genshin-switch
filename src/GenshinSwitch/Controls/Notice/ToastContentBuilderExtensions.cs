using CommunityToolkit.WinUI.Notifications;
using GenshinSwitch.Core;
using Microsoft.UI.Dispatching;

namespace GenshinSwitch.Controls.Notice;

internal static class ToastContentBuilderExtensions
{
    public static ToastContentBuilder AddAttributionTextIf(this ToastContentBuilder builder, bool condition, string text)
    {
        if (condition)
        {
            return builder.AddAttributionText(text);
        }
        else
        {
            return builder.Stub();
        }
    }

    public static ToastContentBuilder Stub(this ToastContentBuilder builder)
    {
        return builder;
    }

    public static void ShowSafe(this ToastContentBuilder builder)
    {
        try
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
            {
                builder.Show();
            });
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
