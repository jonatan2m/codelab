using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PlayBenchmarkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var summary = BenchmarkRunner.Run<SimpleTest>();

        }
    }

    [MemoryDiagnoser]
    public class SimpleTest
    {
        [Params(1000, 1000000)]
        public int N;

        [Benchmark]
        public void ArrayTest()
        {
            int[] _array = new int[N];

            for (int i = 0; i < N; i++)
            {
                _array[i] = i;
            }
        }


    }
}
