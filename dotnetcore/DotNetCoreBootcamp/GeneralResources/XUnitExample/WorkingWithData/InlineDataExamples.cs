using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.WorkingWithData
{

    public class ParameterizedInlineDataTests
    {
        public bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }

        [Theory]
        [InlineData(5, 1, 3, 9)]
        [InlineData(7, 1, 5, 3)]
        public void AllNumbers_AreOdd_WithInlineData(int a, int b, int c, int d)
        {
            Assert.True(IsOddNumber(a));
            Assert.True(IsOddNumber(b));
            Assert.True(IsOddNumber(c));
            Assert.True(IsOddNumber(d));
        }
    }
}
