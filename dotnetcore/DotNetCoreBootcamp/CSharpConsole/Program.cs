using CSharp.Enums.LikeJava;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using CSharp.Threads;
using Newtonsoft.Json;

namespace CSharpConsole
{
    class Teste
    {
        public string a { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Planet planetStatic = Planet.MERCURY;
            Console.WriteLine(planetStatic.SurfaceGravity());

            PlanetEnum planet = PlanetEnum.MERCURY;
            Console.WriteLine(planet.GetSurfaceGravity());

            Performance.Literal_vs_String_Contants.Example01 example = 
                new Performance.Literal_vs_String_Contants.Example01();
            example.CountMemoryAccessingDictionary(100);

            Console.WriteLine("ParseDate");
            example.ParseDate();

            Console.WriteLine("ParseDateWithFormatVariable");
            example.ParseDateWithFormatVariable();

            
            Performance.SelectMany_vs_NestedForeach.Example01 example01 = 
                new Performance.SelectMany_vs_NestedForeach.Example01();

            var sw = new Stopwatch();

            Console.WriteLine("SelectMany");
            sw.Start();
            example01.SelectMany();
            sw.Stop();
            Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");

            sw.Reset();

            Console.WriteLine("NestedForeach");
            sw.Start();
            example01.NestedForeach();
            sw.Stop();
            Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("Hello World!");
            var aa = JsonConvert.DeserializeObject<Teste>("{'aaaa':'a'}");

            new CSharp.Threads.SimpleExample().SplitOperationInTwoThreads();
            new CSharp.Threads.SimpleExample().OperationWithoutThread();

        }
    }
}
