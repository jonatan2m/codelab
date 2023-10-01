using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CSharp.AdvancedCollectionsSpan
{
    //https://github.com/mgravell/blog-preview/blob/main/RefForeach/Program.cs
    public class WorkingWithMarshal
    {        
        public static void ExecuteBenchmarkDotNet()
        {
            BenchmarkRunner.Run<ForEachBenchmark>();
        }
    }

    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net60), MemoryDiagnoser]
    public class ForEachBenchmark
    {
        private readonly List<Foo> _list = new List<Foo>();
        private Foo[] _array;

        [GlobalSetup]
        public void Populate()
        {
            for (int i = 0; i < 1000; i++)
            {
                _list.Add(new Foo(i));
            }
            _array = _list.ToArray();
            //_custom = new CustomWrapper(_array);
        }

        [Benchmark]
        public int ListForEachLoop()
        {
            int total = 0;
            foreach (var tmp in _list)
            {
                total += tmp.SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ArrayForEachLoop()
        {
            int total = 0;
            foreach (var tmp in _array)
            {
                total += tmp.SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ListForLoop()
        {
            int total = 0;
            var snapshot = _list; // (just reduce the field fetches)
            for (int i = 0; i < snapshot.Count; i++)
            {
                total += snapshot[i].SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ArrayForLoop()
        {
            int total = 0;
            var snapshot = _array; // make sure we can elide bounds checks (and also: reduce field fetches)
            for (int i = 0; i < snapshot.Length; i++)
            {
                total += snapshot[i].SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ListLinqSum()
           => _list.Sum(x => x.SomeValue);

        [Benchmark]
        public int ArrayLinqSum()
            => _array.Sum(x => x.SomeValue);

        [Benchmark]
        public int ListForEachMethod()
        {
            int total = 0;
            _list.ForEach(x => total += x.SomeValue);
            return total;
        }

        [Benchmark]
        public int ListRefForeachLoop()
        {
            int total = 0;
            // note: you can do this directly on spans; this code shows
            // how you can get the inner span from a *list* (and why you might want to)
            foreach (ref var tmp in CollectionsMarshal.AsSpan(_list))
            {   // also works identically with "ref readonly var", since this is
                // a readonly struct
                total += tmp.SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ListSpanForLoop()
        {
            int total = 0;
            var span = CollectionsMarshal.AsSpan(_list);
            for (int i = 0; i < span.Length; i++)
            {
                total += span[i].SomeValue;
            }
            return total;
        }

        [Benchmark]
        public int ArrayRefForeachLoop()
        {
            int total = 0;
            foreach (ref var tmp in _array.AsSpan())
            {   // also works identically with "ref readonly var", since this is
                // a readonly struct
                total += tmp.SomeValue;
            }
            return total;
        }


    }

    public class Foo
    {
        public readonly int SomeValue;

        // some other values just to pad things a bit
        public DateTime When { get; }
        public decimal HowMuch { get; }
        public Guid Id { get; }

        public Foo(int someValue)
        {
            SomeValue = someValue;
            When = DateTime.UtcNow;
            Id = Guid.NewGuid();
            HowMuch = 42;
        }
    }
}
