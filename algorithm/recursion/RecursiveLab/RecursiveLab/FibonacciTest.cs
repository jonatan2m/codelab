using Xunit;

namespace RecursiveLab
{
    public class FibonacciTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 3)]
        [InlineData(5, 5)]
        [InlineData(6, 8)]
        [InlineData(7, 13)]        
        [InlineData(20, 6765)]
        public void Should_return_a_fibo_number(int number, int expected)
        {
            Fibonacci fb = new Fibonacci();

            var result = fb.Calculate(number);

            Assert.Equal(expected, result);
        }

        [Theory]        
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 3)]
        [InlineData(5, 5)]
        [InlineData(6, 8)]
        [InlineData(7, 13)]
        [InlineData(20, 6765)]
        public void Should_return_a_fibo_number_by_closed_formula(int number, int expected)
        {
            Fibonacci fb = new Fibonacci();

            var result = fb.CalculateByClosedFormula(number);

            Assert.Equal(expected, result);
        }
    }
}
