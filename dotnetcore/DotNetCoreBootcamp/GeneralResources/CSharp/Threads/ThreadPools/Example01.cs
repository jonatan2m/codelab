using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CSharp.Threads.ThreadPools
{
    public class Example01
    {
        static ConcurrentDictionary<int, int> threads = new ConcurrentDictionary<int, int>();
        public static void Run()
        {
            var ex = new Example01();
            Stopwatch mywatch = new Stopwatch();

            Console.WriteLine("Thread Pool Execution");

            mywatch.Start();
            ex.ProcessWithThreadPoolMethod();
            mywatch.Stop();

            Console.WriteLine("Time consumed by ProcessWithThreadPoolMethod is : " + mywatch.ElapsedTicks.ToString());
            mywatch.Reset();

            foreach (var item in threads)
            {
                System.Console.WriteLine($"{item.Key} = {item.Value} times");
            }

            mywatch.Start();
            //ex.ProcessWithThreadMethod();
            mywatch.Stop();
            
            Console.WriteLine("Time consumed by ProcessWithThreadMethod is : " + mywatch.ElapsedTicks.ToString());            
        }

        public void ProcessWithThreadPoolMethod()
        {
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(callBack: new WaitCallback(Process));
            }
        }

        public void ProcessWithThreadMethod()
        {
            for (int i = 0; i < 20; i++)
            {
                Thread obj = new Thread(Process);
                obj.Start();
            }
        }

        public void Process(object callback)
        {            
            if(threads.ContainsKey(Thread.CurrentThread.ManagedThreadId) == false)
                threads[Thread.CurrentThread.ManagedThreadId] = 0;

            threads[Thread.CurrentThread.ManagedThreadId] += 1;

            Thread.Sleep(1000);
        }
    }
}
