using System;
using System.IO;

namespace DelegatesAndEvents
{
    public class FileLogger
    {
        private readonly string logPath;
        public FileLogger(string path)
        {
            logPath = path;
            Logger.WriteMessage += Logger_WriteMessage;
        }

        public void DetachLog() => Logger.WriteMessage -= Logger_WriteMessage;

        void Logger_WriteMessage(string msg)
        {
            try
            {
                using(var log = File.AppendText(logPath))
                {
                    log.WriteLine(msg);
                    log.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
