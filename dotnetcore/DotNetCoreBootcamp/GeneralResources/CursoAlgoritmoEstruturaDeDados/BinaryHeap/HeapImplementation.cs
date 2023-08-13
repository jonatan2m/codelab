using Microsoft.Diagnostics.Tracing.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.BinaryHeap
{
    /*
     * Não existe implementação nativa para Heap e BinaryHeap dentro do .Net
     * Abaixo segue uma implementação de BinaryHeap
     */
    public class Heap<T>
    {
        //criteria definition
        private readonly Func<T, T, int> priorityCriteria;
        //array to keep the data
        private readonly T[] _data;
        //know the current position
        private int _cursor;

        public Heap(int capacity, Func<T, T, int> priorityCriteria)
        {
            this.priorityCriteria = priorityCriteria;
            _data = new T[capacity];
            _cursor = 0;
        }

        //public
        //Insert
        public void Insert(T value)
        {
            if (_cursor == _data.Length)
            {
                throw new InvalidOperationException("Insert exceeds heap capacity.");
            }

            _data[_cursor++] = value;
            BubbleUp(_cursor - 1);

        }

        //Peek - Always the top element
        public T Peek()
        {
            if (_cursor == 0)
                throw new InvalidOperationException("The heap is empty.");

            return _data[0];
        }

        //Pop - Peek + Remove element
        public T Pop()
        {
            var result = Peek();
            _cursor--;
            //put the last item on the top
            _data[0] = _data[_cursor];
            BubbleDown(0);
            return result;
        }

        //private
        //BubbleUp - Check if item inserted is in the right place
        private void BubbleUp(int position)
        {
            if (HasParent(position) == false)
            {
                return;
            }

            var parent = Parent(position);
            if (priorityCriteria(_data[position], _data[parent]) != -1) return;

            Swap(position, parent);
            BubbleUp(parent);
        }

        private void Swap(int i, int j)
        {
            var cache = _data[i];
            _data[i] = _data[j];
            _data[j] = cache;
        }

        private bool HasParent(int position)
        {
            return Parent(position) != -1;
        }

        private static int Parent(int position)
        {
            if (position == 0) return -1;
            var parent = ((position + 1) / 2);
            return parent - 1;
        }

        //BubbleDown - after an item be removed, it needs to rebalance the tree
        private void BubbleDown(int position)
        {
            if (HasYoungerChild(position) == false) return;

            var c = YoungerChild(position);
            var minIndex = position;

            for (int i = 0; i <= 1; i++)
            {
                if ((c + i) < _cursor)
                {
                    if (priorityCriteria(_data[minIndex], _data[c + i]) == 1)
                    {
                        minIndex = c + i;
                    }
                }
            }

            if (minIndex != position)
            {
                Swap(position, minIndex);
                BubbleDown(minIndex);
            }
        }

        private bool HasYoungerChild(int position)
        {
            return (YoungerChild(position) < _cursor);
        }

        private static int YoungerChild(int position)
        {
            var result = (position + 1) * 2;
            return result - 1;
        }

        public bool HasValue => _cursor > 0;     
    }

    public class MinHeap<T> : Heap<T> where T : IComparable<T>
    {
        public MinHeap(int capacity)
            : base(capacity, (a, b) => a.CompareTo(b))
        { }

        public static T[] MergeOrderedArrays(IList<T[]> inputs)
        {
            var heap = new Heap<ValueTuple<T, int>>(
                inputs.Count,
                (a, b) => a.Item1.CompareTo(b.Item1)
            );

            var indices = new int[inputs.Count];
            var capacity = 0;
            for (var i = 0; i < inputs.Count; i++)
            {
                heap.Insert(new ValueTuple<T, int>(inputs[i][0], i));
                capacity += inputs[i].Length;
            }

            var result = new List<T>(capacity);
            while (heap.HasValue)
            {
                var current = heap.Pop();
                result.Add(current.Item1);
                if (indices[current.Item2] < (inputs[current.Item2].Length - 1))
                {
                    indices[current.Item2]++;
                    heap.Insert(new ValueTuple<T, int>(inputs[current.Item2][indices[current.Item2]], current.Item2));
                }
            }

            return result.ToArray();
        }
    }

    public class MaxHeap<T> : Heap<T> where T : IComparable<T>
    {
        MaxHeap(int capacity)
            : base(capacity, (a, b) => -a.CompareTo(b))
        { }
    }

    public class TestHeap
    {
        [Fact]
        public void TestMinHeap()
        {
            //Usando Binary Heap para mesclar coleções ordenadas

            var inputs = new List<int[]>
    {
        new[] {1, 4, 5, 7},
        new[] {3, 9, 10},
        new[] {2, 6, 8}
    };
            
            
            var merged = MinHeap<int>.MergeOrderedArrays(inputs);

            for (int i = 0; i < merged.Length; i++)
            {
                Console.WriteLine(merged[i]);
            }
        }      
    }
}
