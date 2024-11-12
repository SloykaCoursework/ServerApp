using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp;

public static class Logger
{

    public static void LogErrorTrace(string text) => Console.WriteLine(text);
    public static void LogDebug(string text) => Log(LogLevel.Debug, text);
    public static void LogError(string text) => Log(LogLevel.Error, text);
    public static void LogInformation(string text) => Log(LogLevel.Info, text);
    public static void LogWarning(string text) => Log(LogLevel.Warning, text);
    public static void Log(LogLevel logLevel, string text) => Console.WriteLine($"[{logLevel.ToString(), -7}] {text}");

    public enum LogLevel
    {

        Debug,
        Error,
        Info,
        Warning

    }

}
