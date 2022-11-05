using System.Collections;

namespace GeneralResources.XUnitExample.WorkingWithData
{

    public class TestTheoryDataGenerator1 : TheoryData<int, int, int, int>
    {
        public TestTheoryDataGenerator1()
        {
            Add(5, 1, 3, 9);
            Add(7, 1, 5, 3);

        }
    }

    public class ParameterizedTheoryDataTests1
    {
        public bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }

        [Theory]
        [ClassData(typeof(TestTheoryDataGenerator1))]
        public void AllNumbers_AreOdd_WithClassData(int a, int b, int c, int d)
        {
            Assert.True(IsOddNumber(a));
            Assert.True(IsOddNumber(b));
            Assert.True(IsOddNumber(c));
            Assert.True(IsOddNumber(d));
        }
    }
}
