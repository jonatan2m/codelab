using SOLIDPrinciples.ValidationClass.Example01;
using SOLIDPrinciples.ValidationClass.Example01.BestSolution;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example01
{
    public class OnlyAdultsCanConsumeAlcoholValidationTest
    {
        [Fact]
        public void Person18AndConsumingAlcohol()
        {
            AssertHelper(18, true, true);
        }

        [Fact]
        public void Person17AndConsumingAlcohol()
        {
            AssertHelper(17, true, false);
        }

        [Fact]
        public void Person18AndNotConsumingAlcohol()
        {
            AssertHelper(18, false, true);
        }

        [Fact]
        public void Person17AndNotConsumingAlcohol()
        {
            AssertHelper(17, false, true);
        }

        private void AssertHelper(int age, bool consumesAlcohol, bool expected)
        {
            var person = new Person { Age = age, ConsumesAlcohol = consumesAlcohol };
            var validation = new OnlyAdultsCanConsumeAlcoholValidation(person);
            Assert.Equal(expected, validation.IsValid);
        }
    }
}
