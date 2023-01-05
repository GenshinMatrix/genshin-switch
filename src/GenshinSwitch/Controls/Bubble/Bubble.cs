using GenshinSwitch.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GenshinSwitch.Controls;

/// <summary>
/// Modified version from
/// https://github.com/xunkong/xunkong/blob/41cc491879f22d94da9ee190b341e021565a8e13/App/Helpers/NotificationProvider.cs
/// </summary>
internal static class Bubble
{
    public const int FastTime = 1500;
    public const int NormalTime = 2000;
    public const int SlowTime = 3000;

    private static readonly Timer timer = new(30000);
    private static StackPanel? container;

    public static void Initialize(StackPanel container)
    {
        Bubble.container = container;
        timer.AutoReset = true;
        timer.Elapsed += TimerElapsed;
        timer.Start();
    }

    private static void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (container is null)
        {
            return;
        }
        container.DispatcherQueue?.TryEnqueue(() =>
        {
            var c = container.Children;
            foreach (var item in c)
            {
                var size = item.ActualSize.X * item.ActualSize.Y;
                if (size == 0)
                {
                    c.Remove(item);
                }
            }
        });
    }

    private static void AddInfoBarToContainer(InfoBarSeverity severity, string? title, string? message, int delay)
    {
        if (container is null)
        {
            return;
        }
        container.DispatcherQueue?.TryEnqueue(async () =>
        {
            var infoBar = new InfoBar
            {
                Severity = severity,
                Title = title,
                Message = message,
                IsOpen = true,
                Opacity = 0.9d,
            };
            container.Children.Add(infoBar);
            if (delay > 0)
            {
                await Task.Delay(delay);
                infoBar.IsOpen = false;
            }
        });
    }

    public static void Information(string message, int delay = NormalTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Informational, null, message, delay);
    }

    public static void Information(string title, string message, int delay = NormalTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Informational, title, message, delay);
    }

    public static void Success(string message, int delay = NormalTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Success, null, message, delay);
    }

    public static void Success(string title, string message, int delay = NormalTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Success, title, message, delay);
    }

    public static void Warning(string message, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Warning, null, message, delay);
    }

    public static void Warning(string title, string message, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Warning, title, message, delay);
    }

    public static void Error(string message, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Error, null, message, delay);
    }

    public static void Error(string title, string message, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Error, title, message, delay);
    }

    public static void Error(Exception ex, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Error, ex.GetType().Name, ex.Message, delay);
    }

    public static void Error(Exception ex, string message, int delay = SlowTime)
    {
        AddInfoBarToContainer(InfoBarSeverity.Error, $"{ex.GetType().Name} - {message}", ex.Message, delay);
    }

    public static void Show(InfoBar infoBar, int delay = 0)
    {
        if (container is null)
        {
            return;
        }
        container.DispatcherQueue.TryEnqueue(async () =>
        {
            container.Children.Add(infoBar);
            if (delay > 0)
            {
                await Task.Delay(delay);
                infoBar.IsOpen = false;
            }
        });
    }

    public static void ShowWithButton(InfoBarSeverity severity, string? title, string? message, string buttonContent, Action buttonAction, Action? closedAction = null, int delay = 0)
    {
        if (container is null)
        {
            return;
        }
        container.DispatcherQueue.TryEnqueue(async () =>
        {
            var infoBar = Create(severity, title, message, buttonContent, buttonAction, closedAction);
            container.Children.Add(infoBar);
            if (delay > 0)
            {
                await Task.Delay(delay);
                infoBar.IsOpen = false;
            }
        });
    }

    public static InfoBar Create(InfoBarSeverity severity, string? title, string? message, string? buttonContent = null, Action? buttonAction = null, Action? closedAction = null)
    {
        Button? button = null;
        if (!string.IsNullOrWhiteSpace(buttonContent) && buttonAction != null)
        {
            button = new Button
            {
                Content = buttonContent,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            button.Click += (_, _) =>
            {
                try
                {
                    buttonAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"{title} - {message} - {buttonContent}");
                }
            };
        }
        var infoBar = new InfoBar
        {
            Severity = severity,
            Title = title,
            Message = message,
            ActionButton = button,
            IsOpen = true,
        };
        if (closedAction is not null)
        {
            infoBar.CloseButtonClick += (_, _) =>
            {
                try
                {
                    closedAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"{title} - {message}");
                }
            };
        }
        return infoBar;
    }
}
