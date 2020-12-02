using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Web3_1.AsyncAwait
{
    public class Example01
    {
        //public async Task<int> Run()
        public Task<int> Run()
        {
            Console.WriteLine("Begining of RUN Method!");
            
            int result = 0;

            for (int i = 0; i < 5; i++)
            {
                var task = DoAsyncTask(i);
                var context = SynchronizationContext.Current;
                var ta = task.ContinueWith((t) =>
                {
                    if (context == null)
                    {
                        Console.WriteLine("context is null!");
                    }
                    else
                    {
                        context.Post((obj) => Console.WriteLine($"Ending of RUN Method! ({int.Parse(obj.ToString())}) - ({t.Result})"), null);
                        result += t.Result;
                    }
                }, TaskScheduler.Current).GetAwaiter();
                //result += await task;
            }
            
            return Task.FromResult(result);
        }

        public Task<int> DoAsyncTask(int time)
        {
            Console.WriteLine("Creating a Task DoAsyncTask!");
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"DoAsyncTask({time}) begins!");
                Task.Delay(time * 2000).GetAwaiter().GetResult();
                Console.WriteLine($"DoAsyncTask({time}) Done!");
                return time;
            });
        }
    }
}
