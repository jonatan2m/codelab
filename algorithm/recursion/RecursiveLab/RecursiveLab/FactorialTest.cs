using Xunit;

namespace RecursiveLab
{
    public class FactorialTest
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 6)]
        [InlineData(4, 24)]
        [InlineData(5, 120)]
        [InlineData(10, 3628800)]
        public void Should_return_a_factorial_number(int number, int expected)
        {
            Factorial f = new Factorial();

            var result = f.Calculate(number);

            Assert.Equal(expected, result);
        }
    }
}
