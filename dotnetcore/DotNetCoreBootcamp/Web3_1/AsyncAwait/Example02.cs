using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web3_1.AsyncAwait
{
    public class Example02
    {
        public async Task Run()
        {
            var numbers = new List<int>();
            for (int i = 1; i <= 100; i++)
            {
                numbers.Add(i);
            }

            //await Task.WhenAll(numbers.Select(Write)).ConfigureAwait(false);
            _ = numbers.Select(Write).ToList();
        }

        private async Task Write(int number)
        {
            var r = new Random();
            await Task.Delay(r.Next(0, 10) * 1000).ConfigureAwait(false);


            Console.WriteLine($"{number} {DateTime.Now.Ticks} ({Thread.CurrentThread.Name} - {Thread.CurrentThread.ManagedThreadId})");
        }
    }
}
