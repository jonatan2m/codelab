using SOLIDPrinciples.ValidationClass.Example01;
using SOLIDPrinciples.ValidationClass.Example01.BestSolution;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example01
{
    public class Age0OrHigherValidationTest
    {
        [Fact]
        public void AgeIs0()
        {
            AssertHelper(0, true);
        }

        [Fact]
        public void AgeIsBelow0()
        {
            AssertHelper(-1, false);
        }

        private void AssertHelper(int age, bool expected)
        {
            var person = new Person { Age = age };
            var validation = new Age0OrHigherValidation(person);
            Assert.Equal(expected, validation.IsValid);
        }
    }
}
