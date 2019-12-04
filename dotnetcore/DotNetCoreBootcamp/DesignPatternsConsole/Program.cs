using System;
using System.Collections.Generic;

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
            //DesignPatterns.Command.Example4.ApplicationMakeText.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Memento.Example3.PlayEditor.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Adapter.Example1.PlayAdapter.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Decorator.Example1.PlayDecorator.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Bridge.Example1.PlayView.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Composite.Example1.PlayComposite.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Composite.GiftExample.PlayComposite.Run();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Command.ProductExample.Original.Product.Play();
            Console.WriteLine("--------------------------------------");
            DesignPatterns.Command.ProductExample.V1.Product.Play();


            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Examples from internet");
            Console.WriteLine("--------------------------------------");
            ExamplesFromInternet.AvengersSimulatorHeuristic.PlayAvengers.Run();
            Console.Read();
        }
    }
}
