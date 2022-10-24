using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.Sorting
{
    public class BubbleSort
    {
        public static void Sort(int[] m)
        {
            bool hasChanges = false;
            for (int i = m.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (m[j] > m[j + 1])
                    {
                        SortUtils.Swap(m, j, j + 1);
                        hasChanges = true;
                    }
                }
                if (hasChanges == false) break;

                hasChanges = false;
            }
        }


        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(new int[] { 3, 1, 6, 4, 2 }, new int[] { 1, 2, 3, 4, 6 })]
        public void Validate(int[] input, int[] expected)
        {
            //Arrange
            //Act
            BubbleSort.Sort(input);

            //Assert          
            Assert.Equal(expected, input);
        }
    }

    public class HeapSort
    {
        
    }

    public class BinaryHeap
    {
        List<int> _data = new List<int>();        
       
        /// <summary>
        /// Max-heap
        /// </summary>
        /// <param name="value"></param>
        public void Insert(int value)
        {
            _data.Add(value);
            int i = _data.Count - 1;
            int p = Parent(i);

            while (IsBigger(_data, i, p))
            {
                SortUtils.Swap(_data, i, p);
                i = p;
                p = Parent(i);
            }
        }

        public static void HeapSort(int[] m)
        {
            Heapify(m, m.Length);

            for (int i = m.Length - 1; i >= 0; i--)
            {
                SortUtils.Swap(m, i, 0);
                Heapify(m, i - 1, 0);
            }
        }

        public static BinaryHeap Heapify(IList<int> m, int size)
        {
            if (size <= 1) return default(BinaryHeap);

            //4,2,8,7,1,5,3,6,10
            int lastIndex = size - 1;

            for (int i = lastIndex / 2 - 1; i >= 0; i--)
            {
                Heapify(m, size, i);
            }

            var heap = new BinaryHeap();
            heap._data = new List<int>(m);
            return heap;
        }

        public int Dequeue()
        {
            if(_data.Count == 0) return default(int);

            var elem = _data[0];
            _data[0] = _data[_data.Count - 1];
            _data.RemoveAt(_data.Count - 1);

            Heapify(_data, _data.Count, 0);

            return elem;
        }

        static void Heapify(IList<int> m, int size, int index)
        {
            int biggest = index;
            int l = Left(index);
            int r = Right(index);

            if (l < size && IsBigger(m, l, biggest))
                biggest = l;
            if (r < size && IsBigger(m, r, biggest))
                biggest = r;

            if (biggest != index)
            {
                SortUtils.Swap(m, biggest, index);
                Heapify(m, size, biggest);
            }
        }

        static int Parent(int node)
        {
            return (node - 1) / 2;
        }
        static int Left(int node)
        {
            return 2 * node + 1;
        }

        static int Right(int node)
        {
            return 2 * node + 2;
        }

        static bool IsBigger(IList<int> data, int node1, int node2)
        {
            return data[node1] > data[node2];
        }

        [Fact]
        public void Validate()
        {
            //Arrange
            var array = new int[] { 4, 2, 8, 7, 1, 5, 3, 6, 10 };
            BinaryHeap.HeapSort(array);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 10 }, array);
            //var heap = BinaryHeap.Heapify(array, array.Count());
          
            //Act
            //Assert.Equal(10, heap.Dequeue());
            //Assert.Equal(8, heap.Dequeue());



            //Assert          
            //Assert.Equal(expected, input);
        }
    }
}
