using System;
using System.Collections.Generic;
using System.Text;
using Polly;

namespace CSharpConsole.Resilience.RetryExamples
{
    public class RetryExample01
    {
        public static void RetryUntilReachCount(int retryCount)
        {
            var policy = Policy
                .Handle<Exception>()
                .OrResult<bool>(x => x == false)
                .RetryAsync(retryCount, (ex, count) =>
                {
                    Console.WriteLine($"log ({ex.Exception.Message}) {count}/{retryCount}");
                });

            var taskBotRandom = new TaskBot();

            policy.ExecuteAsync(async () => await taskBotRandom.ExecuteFailureTaskWithExceptionAsync());
        }
    }
}
