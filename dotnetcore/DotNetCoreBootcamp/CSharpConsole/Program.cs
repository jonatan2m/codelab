using CSharp.Enums.LikeJava;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
            //GroupByExample.Run();


            //var bytes = Encoding.ASCII.GetBytes("teste com arquivo");

            //using (var t = File.Create("teste.txt"))
            //{
            //    t.Write(bytes);
            //}

            //using (var source = File.Open("teste.txt", FileMode.Open))
            //{
            //    using (Stream destination = File.Open("teste-copy.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //    {
            //        source.CopyTo(destination);
            //    }
            //}


            //var s = Path.Combine("http://SCCHIB4FIDCVIP:9091/", "upload");

            //var fileInfo = new FileInfo(@"\\buy4sc.local\repository\tesouraria\ReceivableAssignment\QuotationRequest\Summary\CESSAO-20200417-QTRQS-003.rem");
            //var fileName = fileInfo.Name;

            ////fileInfo.DirectoryName


            //Planet planetStatic = Planet.MERCURY;
            //Console.WriteLine(planetStatic.SurfaceGravity());

            //PlanetEnum planet = PlanetEnum.MERCURY;
            //Console.WriteLine(planet.GetSurfaceGravity());

            //Performance.Literal_vs_String_Contants.Example01 example =
            //    new Performance.Literal_vs_String_Contants.Example01();
            //example.CountMemoryAccessingDictionary(100);

            //Console.WriteLine("ParseDate");
            //example.ParseDate();

            //Console.WriteLine("ParseDateWithFormatVariable");
            //example.ParseDateWithFormatVariable();


            //Performance.SelectMany_vs_NestedForeach.Example01 example01 =
            //    new Performance.SelectMany_vs_NestedForeach.Example01();

            //var sw = new Stopwatch();

            //Console.WriteLine("SelectMany");
            //sw.Start();
            //example01.SelectMany();
            //sw.Stop();
            //Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");

            //sw.Reset();

            //Console.WriteLine("NestedForeach");
            //sw.Start();
            //example01.NestedForeach();
            //sw.Stop();
            //Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");

            //Console.WriteLine("Hello World!");
            //var aa = JsonConvert.DeserializeObject<Teste>("{'aaaa':'a'}");

            //var simpleExampleThreadAndTask = new CSharp.Threads.SimpleExample();
            //simpleExampleThreadAndTask.OperationWithoutSplit();
            //simpleExampleThreadAndTask.SplitOperationInTwoThreads();
            //simpleExampleThreadAndTask.SplitOperationInTasks();
            //simpleExampleThreadAndTask.SplitOperationInTasks2();
            //simpleExampleThreadAndTask.SplitOperationParallelFor();
            //simpleExampleThreadAndTask.SplitOperationInThreads();
            //simpleExampleThreadAndTask.CompareResults();

            //var selectMany = new SelectManyExample();
            //selectMany.SelectMany01();

            ////var fileWriterSummary = BenchmarkRunner.Run<FileWriter>();
            //FileWriter.Play();


            ////Start task in background.
            //AutoResetEvent wait = new AutoResetEvent(true);

            //TaskFactoryExample.StartNewExample(async() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        await Task.Delay(1000);
            //        Console.WriteLine(DateTime.Now);
            //    }
            //}, wait);

            //Console.WriteLine("non blocking");

            //Console.WriteLine("Pattern Matching");
            //Console.WriteLine(EliminateIfs.PeakTime(DateTime.Now, true) ==
            //                  EliminateIfs.PeakTimeImperative(DateTime.Now, true));

            //RetryExample01.RetryUntilReachCount(retryCount:5);



            //Console.WriteLine("Enumerables - Yield Return");
            //CSharp.Enumerables.YieldReturn.Example01.Run();
            //CSharp.Enumerables.YieldReturn.Example02.Run();

            //Console.WriteLine("Enumerables - Select and SelectMany");
            //CSharp.Enumerables.SelectAndMany.Example01.Run();

            //Threading
            CSharp.Threads.Example02.Run();

            Console.Read();
        }
    }
}
