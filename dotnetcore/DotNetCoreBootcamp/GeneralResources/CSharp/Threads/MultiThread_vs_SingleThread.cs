using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GeneralResources.CSharp.Threads
{
    public class MultiThread_vs_SingleThread
    {
        ITestOutputHelper _output;

        public MultiThread_vs_SingleThread(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// N = 1000000
        /// Given the cost to find available threads, if N is too small, Parallel Foreach doesn't worth it.
        //Parallel Foreach: 940 (ms)
        //Foreach: 1714 (ms)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Example01()
        {
            var numbers = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();

            var watch = new Stopwatch();

            _output.WriteLine("ThreadPool.ThreadCount: " + ThreadPool.ThreadCount.ToString());

            watch.Start();
            var parallelForEach = Parallel.ForEach(numbers, (i) => _output.WriteLine(i));
            watch.Stop();

            _output.WriteLine("ThreadPool.ThreadCount: " + ThreadPool.ThreadCount.ToString());

            long parallelForeach = watch.ElapsedMilliseconds;

            watch.Restart();
            foreach (var item in numbers)
            {
                _output.WriteLine(item);
            }
            watch.Stop();

            long @foreach = watch.ElapsedMilliseconds;
            _output.WriteLine($"Parallel Foreach: {parallelForeach} (ms)");
            _output.WriteLine($"Foreach: {@foreach} (ms)");
        }

        [Fact]
        public async Task Example02()
        {
            _output.WriteLine($"Start: Thread {Thread.CurrentThread.ManagedThreadId}");

            var f = (int i) =>
            {
                _output.WriteLine($"Task{i}: Thread {Thread.CurrentThread.ManagedThreadId}");
                //if (i == 3)
                //{
                //    Thread.Sleep(1000 * i);
                //    return Task.CompletedTask;
                //}
                //else
                    return Task.Delay(1000 * i);
            };

            var tasks = Enumerable.Range(1, 5).Select(i => f(i));

            await Task.WhenAll(tasks);

            _output.WriteLine($"End: Thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
