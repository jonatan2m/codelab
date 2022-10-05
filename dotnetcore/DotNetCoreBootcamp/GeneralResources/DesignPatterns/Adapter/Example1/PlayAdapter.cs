using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Adapter.Example1
{
    public class PlayAdapter
    {
        public static void Run()
        {
            Adaptee adaptee = new Adaptee();
            ITarget target = new Adapter(adaptee);

            Console.WriteLine("Adaptee interface is incompatible with the client.");
            Console.WriteLine("But with adapter client can call it's method.");

            Console.WriteLine(target.GetRequest());
        }
    }
}
