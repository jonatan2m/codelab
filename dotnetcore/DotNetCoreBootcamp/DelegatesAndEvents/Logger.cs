using System;
using System.Threading;

namespace DelegatesAndEvents
{
    public enum Severity
    {
        Verbose,
        Trace,
        Information,
        Warning,
        Error,
        Critical
    }
    public static class Logger
    {
        public static Action<string> WriteMessage;

        public static Severity LogLevel { get; set; } = Severity.Warning;

        public static void LogMessage(Severity s, string component, string msg)
        {
            if (s < LogLevel) return;

            var output = $"{DateTime.Now}\t{s}\t{component}\t{msg}";
            WriteMessage?.Invoke(output);
        }

        //Only to debug
        static void create()
        { 
            var console = new ConsoleLogger();
            var file = new FileLogger("log.txt");
        }

        public static void Run()
        {
            Logger.LogLevel = Severity.Information;

            create();

            var logLevelRandom = new Random();
            var logLevels = Enum.GetValues(typeof(Severity));

            //Logger.Cancel();

            for (int i = 0; i < 10; i++)
            {
               var logLevel = logLevels.GetValue(logLevelRandom.Next(logLevels.Length - 1));
               Logger.LogMessage((Severity)logLevel, "Main", "Meu teste maroto");
               //GC.Collect();
            }

        }

        private static void Cancel()
        {
            WriteMessage = null;
        }
    }
}
