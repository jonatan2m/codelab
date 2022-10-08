using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CSharp.Threads
{
    public class MultiThread_vs_SingleThread
    {
        [Fact]
        public async Task Example01()
        {
            var numbers = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();

            var watch = new Stopwatch();
            watch.Start();

            var parallelForEach = Parallel.ForEach(numbers, (i) => Console.WriteLine(i));

            watch.Stop();
            Debug.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}
