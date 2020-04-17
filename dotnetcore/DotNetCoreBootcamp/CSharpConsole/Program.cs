using CSharp.Enums.LikeJava;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using CSharp.FileGenerate;
using CSharp.LINQ;
using CSharp.PatternMatching;
using CSharp.Threads;
using CSharpConsole.Resilience.RetryExamples;
using CSharpConsole.TasksExamples;
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

            var simpleExampleThreadAndTask = new CSharp.Threads.SimpleExample();
            simpleExampleThreadAndTask.OperationWithoutSplit();
            simpleExampleThreadAndTask.SplitOperationInTwoThreads();
            simpleExampleThreadAndTask.SplitOperationInTasks();
            simpleExampleThreadAndTask.SplitOperationInThreads();
            simpleExampleThreadAndTask.CompareResults();

            var selectMany = new SelectManyExample();
            selectMany.SelectMany01();

            //var fileWriterSummary = BenchmarkRunner.Run<FileWriter>();
            FileWriter.Play();


            //Start task in background.
            AutoResetEvent wait = new AutoResetEvent(true);
            
            TaskFactoryExample.StartNewExample(async() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    Console.WriteLine(DateTime.Now);
                }
            }, wait);

            Console.WriteLine("non blocking");

            Console.WriteLine("Pattern Matching");
            Console.WriteLine(EliminateIfs.PeakTime(DateTime.Now, true) ==
                              EliminateIfs.PeakTimeImperative(DateTime.Now, true));
            
            RetryExample01.RetryUntilReachCount(retryCount:5);

            Console.Read();
        }
    }
}
