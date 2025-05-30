﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSharp.Threads
{
    public class Example02
    {
        public static void Run()
        {
            var ex = new Example02();

            ex.WithoutThreading();

            //ex.WithThreading();

            //ex.WithJoinThreading();

            //ex.WithSleepThreading();

            //ex.ForegroundThread();

            //ex.BackgroundThread();
        }

        public void WithSleepThreading()
        {
            Console.WriteLine(nameof(WithSleepThreading));

            Thread t1 = new Thread(() => Work1(nameof(WithSleepThreading)));
            Thread t2 = new Thread(() => Work2(nameof(WithSleepThreading)));

            t1.Start();
            Thread.Sleep(3000);
            t2.Start();
            Thread.Sleep(3000);
            Thread.Sleep(3000);


            Console.WriteLine("--- END");
        }


        /// <summary>
        /// Thread.Join will executed the attached thread immediately
        /// </summary>
        public void WithJoinThreading()
        {
            Console.WriteLine(nameof(WithJoinThreading));

            Thread t1 = new Thread(() => Work1(nameof(WithJoinThreading)));
            Thread t2 = new Thread(() => Work2(nameof(WithJoinThreading)));

            t1.Start();
            t2.Start();
            t2.Join();

            Console.WriteLine($"{nameof(WithJoinThreading)} --- END");
        }

        public void WithThreading()
        {
            Console.WriteLine(nameof(WithThreading));

            Thread t1 = new Thread(() => Work1(nameof(WithThreading)));
            Thread t2 = new Thread(() => Work2(nameof(WithThreading)));

            t1.Start();
            t2.Start();

            Console.WriteLine($"{nameof(WithThreading)} --- END");
        }

        public void WithoutThreading()
        {
            Console.WriteLine(nameof(WithoutThreading));

            Work1(nameof(WithoutThreading));
            Work2(nameof(WithoutThreading));

            Console.WriteLine($"{nameof(WithoutThreading)} --- END");
        }

        public void BackgroundThread()
        {
            Console.WriteLine(nameof(BackgroundThread));

            Thread t1 = new Thread(() => WorkSleeping(nameof(BackgroundThread)));

            t1.IsBackground = true;

            t1.Start();

            Console.WriteLine($"{nameof(BackgroundThread)} --- END");
        }

        //It will run event the main thread finishes its processing
        public void ForegroundThread()
        {
            Console.WriteLine(nameof(ForegroundThread));

            Thread t1 = new Thread(() => WorkSleeping(nameof(ForegroundThread)));
            
            t1.Start();

            Console.WriteLine($"{nameof(ForegroundThread)} --- END");
        }

        private void Work1(string prefix)
        {
            Thread th = Thread.CurrentThread;
            for (int i = 1; i <= 100000; i++)
            {
                if (i % 10000 == 0)
                    Console.WriteLine($"{prefix}_({th.ManagedThreadId}){nameof(Work1)} is called {i}");
            }
        }

        private void Work2(string prefix)
        {
            Thread th = Thread.CurrentThread;
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{prefix}_({th.ManagedThreadId}){nameof(Work2)} is called {i}");
            }
        }

        private void WorkSleeping(string prefix)
        {
            Thread th = Thread.CurrentThread;
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"{prefix}_({th.ManagedThreadId}){nameof(Work1)} is called {i}");
                Thread.Sleep(2000);
            }
        }
    }
}
