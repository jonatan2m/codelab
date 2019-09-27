using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CSharp.Threads
{
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
        private List<int> resultWithoutSplit;
        private Foo foo = new Foo();
        public SimpleExample()
        {
            resultTask = new List<int>();
            resultTwoThread = new List<int>();
            resultThread = new List<int>();
            resultWithoutSplit = new List<int>();

            data = new List<int>();
            for (int i = 0; i < 10; i++)
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
            Console.WriteLine($"Two Threads: time-> {st.ElapsedMilliseconds}");
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
            Console.WriteLine($"Thread time-> {st.ElapsedMilliseconds}");
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
            Console.WriteLine($"Tasks time-> {st.ElapsedMilliseconds}");
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
            Console.WriteLine($"WithoutSplit time-> {st.ElapsedMilliseconds}");
        }

        public void CompareResults()
        {
                if(resultTwoThread.Count == resultTask.Count && resultWithoutSplit.Count == resultTask.Count
                && resultThread.Count == resultTask.Count)
                    Console.WriteLine($"OK");
                else
                {
                    Console.WriteLine("NOK");
                }
         
        }
    }
}
