using System;
namespace DesignPatterns.ChainOfResponsability.Example1
{
    public class SubtractNumbers : IChain
    {
        public SubtractNumbers()
        {

        }
        public SubtractNumbers(IChain next)
        {
            Next = next;

        }
        public IChain Next { get; set; }

        public double Calculate(Numbers numbers)
        {
            if (numbers.CalcOperator == CalcOperator.Sub)
                return numbers.Number1 - numbers.Number2;
            else
            {
                if (Next != null) return Next.Calculate(numbers);
                return 0;
            }

        }

        public void SetNext(IChain next)
        {
            Next = next;
        }
    }
}
