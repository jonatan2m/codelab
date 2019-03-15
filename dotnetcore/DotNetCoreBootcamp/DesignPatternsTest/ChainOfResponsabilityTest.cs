using System;
using DesignPatterns.ChainOfResponsability.Example1;
using Xunit;

namespace DesignPatternsTest
{
    public class ChainOfResponsabilityTest
    {
       [Fact]
       public void ChainOfResponsability_Example1()
        {
            DivNumbers div = new DivNumbers();
            MultNumbers mult = new MultNumbers(div);
            SubtractNumbers sub = new SubtractNumbers(mult);
            AddNumbers rootOperator = new AddNumbers(sub);

            Numbers numbers = new Numbers(2, 4, CalcOperator.Mult);
            var result = rootOperator.Calculate(numbers);

            Assert.Equal(8, result);
        }
    }
}
