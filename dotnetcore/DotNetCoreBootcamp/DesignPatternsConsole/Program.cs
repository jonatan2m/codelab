using System;

namespace DesignPatternsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DesignPatterns.Command.Example1.PlayWithRemote.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Command.Example3.PlayRobot.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Command.Example4.ApplicationMakeText.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Memento.Example3.PlayEditor.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Adapter.Example1.PlayAdapter.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Decorator.Example1.PlayDecorator.Run();

            Console.Read();
        }
    }
}
