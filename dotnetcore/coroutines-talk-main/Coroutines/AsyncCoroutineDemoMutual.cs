﻿// https://github.com/noseratio/coroutines-talk

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Coroutines
{
    public static class AsyncCoroutineDemoMutual
    {
        private const int intervalMs = 50;

        private static async IAsyncEnumerable<int> CoroutineA(
            IAsyncCoroutineProxy<int> coroutineProxy,
            [EnumeratorCancellation] CancellationToken token)
        {
            var proxyB = await coroutineProxy.AsAsyncEnumerable(token);
            var interval = new Interval();

            // await for coroutineB to advance by 40 steps
            await foreach (var stepB in proxyB)
            {
                if (stepB >= 40) break;
                Console.SetCursorPosition(0, 0);
                // display a throber
                Console.Write($"{nameof(CoroutineA)}: {@"-\|/"[stepB % 4]}");
                await interval.Delay(intervalMs, token);
            }

            // now do our own 80 steps 
            for (int i = 0; i < 80; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"{nameof(CoroutineA)}: {new String('A', i)}");

                await interval.Delay(intervalMs, token);
                yield return i;
            }
        }

        /// <summary>
        /// CoroutineB yields to CoroutineA
        /// </summary>
        private static async IAsyncEnumerable<int> CoroutineB(
            IAsyncCoroutineProxy<int> coroutineProxy,
            [EnumeratorCancellation] CancellationToken token)
        {
            var proxyA = await coroutineProxy.AsAsyncEnumerable(token);
            var interval = new Interval();

            for (int i = 0; i < 80; i++)
            {
                Console.SetCursorPosition(0, 1);
                Console.Write($"{nameof(CoroutineB)}: {new String('B', i)}");

                await interval.Delay(intervalMs, token);
                yield return i;

                if (i == 40)
                {
                    await proxyA.Take(40).LastOrDefaultAsync();

                    // TODO: demo async Linq!

                    // await for 40 steps of CoroutineA to catch up
                    //await foreach (var stepA in proxyA)
                    //{
                    //    if (stepA >= 40) break;
                    //    Console.SetCursorPosition(0, 1);
                    //    // display a throber
                    //    Console.Write($"{nameof(CoroutineB)}: {new String('B', i)}{@"-\|/"[stepA % 4]}");
                    //    await interval.Delay(intervalMs, token);
                    //}
                }
            }
        }

        private static async Task DriveCoroutinesAsync(CancellationToken token)
        {
            var proxyA = new AsyncCoroutineProxy<int>();
            var proxyB = new AsyncCoroutineProxy<int>();

            // start both coroutines
            await Task.WhenAll(
                proxyA.RunAsync(token => CoroutineA(proxyB, token), token),
                proxyB.RunAsync(token => CoroutineB(proxyA, token), token));
        }

        public static async Task DemoAsync(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                Console.Clear();
                await DriveCoroutinesAsync(token);
            }
        }
    }
}
