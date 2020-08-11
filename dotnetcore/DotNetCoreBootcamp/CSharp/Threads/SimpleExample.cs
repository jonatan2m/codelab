using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Diagnostics.Runtime.DacInterface;

namespace CSharp.Threads
{
    /// <summary>
    /// https://docs.microsoft.com/pt-br/dotnet/api/system.collections.concurrent.blockingcollection-1?view=netcore-3.1
    /// </summary>
    public class Foo
    {
        public int Number { get; set; } = 0;

        public int Process(string prefix, int a)
        {
            int magicNumber = 40;
            while (Number < magicNumber)
            {
                for (int i = 0; i < magicNumber; i++)
                    for (int j = 0; j < magicNumber; j++)
                        for (int l = 0; l < magicNumber; l++)
                            for (int m = 0; m < magicNumber; m++)
                                for (int n = 0; n < magicNumber; n++)
                                {
                                    Number++;
                                }
            }

            return a;
        }
    }

    public class SimpleExample
    {
        private List<int> data;
        private List<int> resultTwoThread;
        private List<int> resultThread;
        private List<int> resultTask;
        private List<int> resultTask2;
        private List<int> resultWithoutSplit;
        private List<int> resultParallelFor;
        private Foo foo = new Foo();

        private const int LENGHT = 1000;

        public SimpleExample()
        {
            resultTask = new List<int>(LENGHT);
            resultTask2 = new List<int>(LENGHT);
            resultTwoThread = new List<int>(LENGHT);
            resultThread = new List<int>(LENGHT);
            resultWithoutSplit = new List<int>(LENGHT);
            resultParallelFor = new List<int>(LENGHT);

            data = new List<int>(LENGHT);

            for (int i = 0; i < LENGHT; i++)
            {
                data.Add(i);
            }
        }

        public void SplitOperationInTwoThreads()
        {
            var part1 = data.Take(data.Count / 2);
            var part2 = data.Skip(data.Count / 2);
            Stopwatch st = Stopwatch.StartNew();
            Console.WriteLine("inicia processamento das threads");
            Thread thread1 = new Thread(() =>
            {
                resultTwoThread.AddRange(part1.Select(x => foo.Process("Threads 1", x)));
            });

            Thread thread2 = new Thread(() =>
            {
                resultTwoThread.AddRange(part2.Select(x => foo.Process("Threads 2", x)));
            });

            thread1.Start();
            thread2.Start();

            while (thread1.IsAlive || thread2.IsAlive) ;

            st.Stop();
            Console.WriteLine($"Two Threads: time-> {st.ElapsedMilliseconds} Items[{resultTwoThread.Count}]");
        }

        public void SplitOperationInThreads()
        {
            Stopwatch st = Stopwatch.StartNew();

            foreach (var item in data)
            {
                var thread = new Thread(() =>
                {
                    resultThread.Add(foo.Process("Threads", item));
                });
                thread.Start();
            }
            //Do Not Work
            //var threads = data.Select(x =>
            //{
            //    var thread = new Thread(() =>
            //    {
            //        resultThread.Add(foo.Process("Threads", x));
            //    });
            //    thread.Start();

            //    return thread;
            //});

            st.Stop();
            Console.WriteLine($"Thread time-> {st.ElapsedMilliseconds}. Items[{resultThread.Count}]");
        }

        public void SplitOperationInTasks()
        {
            Stopwatch st = Stopwatch.StartNew();
            var processTasks = data.Select(x =>
            {
                return Task.Factory.StartNew(() =>
                {
                    resultTask.Add(foo.Process("Tasks", x));
                });
            }).ToArray();
            Task.WaitAll(processTasks);

            st.Stop();
            Console.WriteLine($"Tasks time-> {st.ElapsedMilliseconds}. Items[{resultTask.Count}]");
        }

        public void OperationWithoutSplit()
        {
            Stopwatch st = Stopwatch.StartNew();
            for (int i = 0; i < data.Count; i++)
            {
                resultWithoutSplit.Add(foo.Process("NoSplitation", data[i]));
            }
            //data.Select(x => foo.Process("NoSplitation"));
            st.Stop();
            Console.WriteLine($"WithoutSplit time-> {st.ElapsedMilliseconds}. Items[{resultWithoutSplit.Count}]");
        }

        public void SplitOperationInTasks2()
        {
            var part1 = data.Take(data.Count / 2);
            var part2 = data.Skip(data.Count / 2);

            Stopwatch st = Stopwatch.StartNew();

            Task t1 = Task.Run(() =>
            {
                int count = 0;
                Console.WriteLine($"t1 ({count}) {resultTask2.Count}");
                foreach (int item in part1)
                {
                    resultTask2.Add(foo.Process("Tasks", item));
                    count++;
                }
                Console.WriteLine($"t1 ({count}) {resultTask2.Count}");
            });

            Task t2 = Task.Run(() =>
            {
                int count = 0;
                Console.WriteLine($"t2 ({count}) {resultTask2.Count}");
                foreach (int item in part2)
                {
                    resultTask2.Add(foo.Process("Tasks", item));
                    count++;
                }
                Console.WriteLine($"t2 ({count}) {resultTask2.Count}");

            });

            var t0 = Task.Factory.ContinueWhenAll(new[] {t1, t2}, (tasks) =>
            {
                st.Stop();
                Console.WriteLine($"t1 {t1.Status}; t2 {t2.Status}");

            });

            t0.Wait();

            Console.WriteLine($"Tasks time (2) -> {st.ElapsedMilliseconds}. Items[{resultTask2.Count}]");

            Console.WriteLine($"task2 ---");
        }

        public void SplitOperationParallelFor()
        {
            Stopwatch st = Stopwatch.StartNew();

            var rangePartitioner = Partitioner.Create(0, data.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    resultParallelFor.Add(foo.Process("NoSplitation", data[i]));
                }
            });

            st.Stop();
            Console.WriteLine($"SplitOperationParallelFor time-> {st.ElapsedMilliseconds}. Items[{resultParallelFor.Count}]");
        }

        public void CompareResults()
        {
            if (resultTwoThread.Count == resultTask.Count &&
               resultWithoutSplit.Count == resultTask.Count &&
               resultThread.Count == resultTask.Count)
                Console.WriteLine($"OK");
            else
            {
                Console.WriteLine("NOK");
            }
        }
    }
}
