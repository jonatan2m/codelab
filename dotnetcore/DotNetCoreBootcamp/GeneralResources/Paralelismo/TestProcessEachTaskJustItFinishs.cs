using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.Paralelismo
{
    /// <summary>
    /// Implementing examples from this article:
    /// https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
    /// </summary>
    public class TestProcessEachTaskJustItFinishs
    {        
        public static async Task Test1_OK()
        {
            var tasks = new[] {
    Task.Delay(3000).ContinueWith(_ => 3),
    Task.Delay(1000).ContinueWith(_ => 1),
    Task.Delay(2000).ContinueWith(_ => 2),
    Task.Delay(5000).ContinueWith(_ => 5),
    Task.Delay(4000).ContinueWith(_ => 4),
};
            foreach (var bucket in Interleaved(tasks))
            {
                var t = await bucket;
                int result = await t;
                Console.WriteLine($"{DateTime.Now}: {result}");
            }
        }

        public static async Task Test1_NOK()
        {
            var tasks = new[] {
    Task.Delay(3000).ContinueWith(_ => 3),
    Task.Delay(1000).ContinueWith(_ => 1),
    Task.Delay(2000).ContinueWith(_ => 2),
    Task.Delay(5000).ContinueWith(_ => 5),
    Task.Delay(4000).ContinueWith(_ => 4),
};
            foreach (var t in tasks)
            {
                int result = await t;
                Console.WriteLine($"{DateTime.Now}: {result}");
            }
        }

        public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
        {
            var inputTasks = tasks.ToList();

            var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
            var results = new Task<Task<T>>[buckets.Length];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task<T>>();
                results[i] = buckets[i].Task;
            }

            int nextTaskIndex = -1;
            Action<Task<T>> continuation = completed =>
            {
                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            };

            foreach (var task in inputTasks)            
                task.ContinueWith(
                    continuation,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);

            return results;
        }
    }

}
