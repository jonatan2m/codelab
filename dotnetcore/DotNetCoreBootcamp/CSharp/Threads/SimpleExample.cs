using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharp.Threads
{
    public class Foo
    {
        public int Number { get; set; } = 0;

        public bool Process()
        {
            while (Number < 100000) Number++;

            return true;
        }
    }

    public class SimpleExample
    {
        private List<Foo> data;

        public SimpleExample()
        {
            data = new List<Foo>();
            for (int i = 0; i < 100000000; i++)
            {
                data.Add(new Foo());
            }
        }

        public void SplitOperationInTwoThreads()
        {
            var part1 = data.Take(data.Count / 2);
            var part2 = data.Skip(data.Count / 2);

            Thread thread1 = new Thread(() =>
            {
                Stopwatch st = Stopwatch.StartNew();

                part1.Select(x => x.Process());
                st.Stop();
                Console.WriteLine($"Thread 1: time-> {st.ElapsedMilliseconds}");
            });

            Thread thread2 = new Thread(() =>
            {
                Stopwatch st = Stopwatch.StartNew();

                part2.Select(x => x.Process());
                st.Stop();
                Console.WriteLine($"Thread 2: time-> {st.ElapsedMilliseconds}");
            });
        }

        public void OperationWithoutThread()
        {
            Stopwatch st = Stopwatch.StartNew();

            data.Select(x => x.Process());

            st.Stop();

            Console.WriteLine($"No Split Thread: time-> {st.ElapsedMilliseconds}");
        }
    }
}
