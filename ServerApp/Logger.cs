using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public static class Logger
    {

        public static void LogError(Exception e)
        {
            LogError(e.Message);
            LogErrorTrace(e.StackTrace!);
        }

        public static void LogErrorTrace<T>(T? text) => Console.WriteLine(text?.ToString());
        public static void LogDebug<T>(T? text) => Log(LogLevel.Debug, text?.ToString());
        public static void LogError<T>(T? text) => Log(LogLevel.Error, text?.ToString());
        public static void LogInformation<T>(T? text) => Log(LogLevel.Info, text?.ToString());
        public static void LogWarning<T>(T? text) => Log(LogLevel.Warning, text?.ToString());
        public static void Log<T>(LogLevel logLevel, T? text) => Console.WriteLine($"[{logLevel.ToString(), -7}] {text?.ToString()}");

        public enum LogLevel
        {

            Debug,
            Error,
            Info,
            Warning

        }

    }
}
