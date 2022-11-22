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

    //public class CalculatorTests
    //{
    //    [Theory]
    //    [MemberData(nameof(CalculatorData.Data), MemberType = typeof(CalculatorData))]
    //    public void CanAddTheoryMemberDataMethod(int value1, int value2, int expected)
    //    {
    //        var calculator = new Calculator();

    //        var result = calculator.Add(value1, value2);

    //        Assert.Equal(expected, result);
    //    }
    //}

    //public class CalculatorData
    //{
    //    public static IEnumerable<object[]> Data =>
    //        new List<object[]>
    //        {
    //        new object[] { 1, 2, 3 },
    //        new object[] { -4, -6, -10 },
    //        new object[] { -2, 2, 0 },
    //        new object[] { int.MinValue, -1, int.MaxValue },
    //        };
    //}
}
