﻿using GenshinSwitch.Core.Services;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;

namespace GenshinSwitch.Core;

public static class Logger
{
    private static readonly FileSystem FileSystem = new();
    private static readonly IPath Path = FileSystem.Path;
    private static readonly IDirectory Directory = FileSystem.Directory;
    private static readonly string ApplicationLogPath = SpecialPathService.Provider.GetFolder();
    private static readonly TextWriterTraceListener TraceListener = null!;

    static Logger()
    {
        if (!Directory.Exists(ApplicationLogPath))
        {
            Directory.CreateDirectory(ApplicationLogPath);
        }

        string logFilePath = Path.Combine(ApplicationLogPath, DateTime.Now.ToString(@"yyyyMMdd", CultureInfo.InvariantCulture) + ".log");
        TraceListener = new TextWriterTraceListener(logFilePath);
#if LEGACY
        Trace.AutoFlush = true;
        Trace.Listeners.Clear();
        Trace.Listeners.Add(TraceListener);
#endif
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static void Ignore(params object[] values)
    {
    }

    public static void Info(params object[] values)
    {
        Log("INFO", string.Join(" ", values));
    }

    public static void Warn(params object[] values)
    {
        Log("ERROR", string.Join(" ", values));
    }

    public static void Error(params object[] values)
    {
        Log("ERROR", string.Join(" ", values));
    }

    public static void Fatal(params object[] values)
    {
        Log("FATAL", string.Join(" ", values));
#if DEBUG && false
        Debugger.Break();
#endif
    }

    public static void Exception(Exception e, string message = null!)
    {
        Log(
            (message ?? string.Empty) + Environment.NewLine +
            e?.Message + Environment.NewLine +
            "Inner exception: " + Environment.NewLine +
            e?.InnerException?.Message + Environment.NewLine +
            "Stack trace: " + Environment.NewLine +
            e?.StackTrace,
            "ERROR");
#if DEBUG
        Debugger.Break();
#endif
    }

    private static void Log(string type, string message)
    {
        StringBuilder sb = new();

        sb.Append(type + "|" + DateTime.Now.ToString(@"yyyy-MM-dd|HH:mm:ss.fff", CultureInfo.InvariantCulture))
          .Append("|" + GetCallerInfo())
          .Append("|" + message);

        Debug.WriteLine(sb.ToString());
#if true // Always on
        TraceListener.WriteLine(sb.ToString());
        TraceListener.Flush();
#endif
    }

    private static string GetCallerInfo()
    {
        StackTrace stackTrace = new();

        MethodBase methodName = stackTrace.GetFrame(3)?.GetMethod()!;
        string? className = methodName?.DeclaringType?.Name;
        return className + "|" + methodName?.Name;
    }
}
