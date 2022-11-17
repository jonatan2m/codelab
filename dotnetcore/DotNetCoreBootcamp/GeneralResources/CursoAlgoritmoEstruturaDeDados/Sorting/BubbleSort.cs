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

   
}
