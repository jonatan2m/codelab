namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.Sorting
{
    /// <summary>
    /// https://www.techopedia.com/definition/21578/quicksort
    /// </summary>
    public class QuickSort
    {
        /* Description
         * 
The quicksort algorithm is performed as follows:

A pivot point is chosen from the array.

The array is reordered so that all values smaller than the pivot are moved before it and all values larger than the pivot are moved after it, with values equaling the pivot going either way. When this is done, the pivot is in its final position.

The above step is repeated for each subarray of smaller values as well as done separately for the subarray with greater values.
This is repeated until the entire array is sorted.
         */
               

        /// <summary>
        /// The pivot choice is the most important step in this algorithm
        /// </summary>
        /// <param name="m"></param>
        public static void Sort(int[] m)
        {
            Sort(m, 0, m.Length - 1);
        }

        static void Sort(int[] m, int startPos, int endPos)
        {
            //pivot choice
            int pivot = m[startPos];
            int l = startPos;
            int r = endPos;

            while (l <= r)
            {
                //stop if any point break the rule
                while (m[l] < pivot) l++;
                while (m[r] > pivot) r--;

                if (l <= r)
                {
                    SortUtils.Swap(m, l, r);
                    l++;
                    r--;
                }
            }

            if(startPos < r) Sort(m, startPos, r);
            if(endPos > l) Sort(m, l, endPos);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(new int[] { 3, 1, 6, 4, 2 }, new int[] { 1, 2, 3, 4, 6 })]
        public void Validate(int[] input, int[] expected)
        {
            //Arrange
            //Act
            QuickSort.Sort(input);

            //Assert          
            Assert.Equal(expected, input);
        }
    }
}
