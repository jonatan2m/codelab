using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace CSharpConsole.Performance.Literal_vs_String_Contants
{
    /// <summary>
    /// https://stackoverflow.com/questions/2587694/performance-of-string-literals-vs-constants-for-session-dictionary-keys    /// 
    /// Não faz diferença
    /// 
    /// https://www.dotnetperls.com/dictionary-string-key
    /// Aqui tem vários casos interessantes sobre performance quando trabalhamos com dicionários
    /// </summary>
    public class Example01
    {
        Dictionary<string, int> test = new Dictionary<string, int>();

        public void CountMemoryAccessingDictionary(int amount)
        {
            var index = "AAAAAA";
            test.Add("AAAAAA", 1);

            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Tempo Inicio: {sw.ElapsedMilliseconds}ms");
            var before2 = GC.CollectionCount(2);
            var before1 = GC.CollectionCount(1);
            var before0 = GC.CollectionCount(0);

            for (int i = 0; i < amount; i++)
            {
                test[index] = 1 + 1;
                //var tt = "AAAAAA";
            }

            sw.Stop();

            Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"GC Gen #2  : {GC.CollectionCount(2) - before2}");
            Console.WriteLine($"GC Gen #1  : {GC.CollectionCount(1) - before1}");
            Console.WriteLine($"GC Gen #0  : {GC.CollectionCount(0) - before0}");
            Console.WriteLine("Done!");
        }

        public void ParseDate()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100000; i++)
            {
                DateTime.TryParseExact(DateTime.Now.ToLongDateString(),
                                "yyyyMMdd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var maturityDate);

            }
            sw.Stop();
            Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");
        }

        public void ParseDateWithFormatVariable()
        {
            var sw = new Stopwatch();
            var formatDate = "yyyyMMdd";
            sw.Start();
            for (int i = 0; i < 100000; i++)
            {
                DateTime.TryParseExact(DateTime.Now.ToLongDateString(),
                                formatDate,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var maturityDate);

            }
            sw.Stop();
            Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");
        }
    }
}
