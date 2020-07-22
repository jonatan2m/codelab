using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Enumerables.YieldReturn
{

    /// <summary>
    /// Based on Elemar's talk about yield return
    /// https://www.youtube.com/watch?v=TbvD3BwC01U
    /// </summary>
    public static class Example01
    {
        public static void Run()
        {
            Console.WriteLine("Before Foo");
            var foo = Foo();
            Console.WriteLine("After Foo");

            foreach (var item in foo)
            {
                Console.WriteLine($"Before printing item {item}");
                Console.WriteLine(item);
                Console.WriteLine($"After printing item {item}");
            }
        }

        private static IEnumerable<int> Foo()
        {
            Console.WriteLine("Before starting loop for");

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Before yield return {i}");
                yield return i;
                Console.WriteLine($"After yield return {i}");
            }

            Console.WriteLine("After finishing loop for");
        }
    }
}
