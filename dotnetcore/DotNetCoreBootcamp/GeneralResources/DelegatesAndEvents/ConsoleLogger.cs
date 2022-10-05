using System;
namespace DelegatesAndEvents
{
    public class ConsoleLogger
    {
        public ConsoleLogger()
        {
            Logger.WriteMessage += Logger_WriteMessage;
        }

        public void DetachLog() => Logger.WriteMessage -= Logger_WriteMessage;

        void Logger_WriteMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        ~ConsoleLogger()
        {
            Console.WriteLine("GC passando");
        }
    }
}
