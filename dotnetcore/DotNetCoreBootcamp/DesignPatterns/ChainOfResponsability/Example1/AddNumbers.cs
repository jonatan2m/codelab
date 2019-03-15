using System;
namespace DesignPatterns.ChainOfResponsability.Example1
{
    public class AddNumbers : IChain
    {
        public AddNumbers()
        {

        }
        public AddNumbers(IChain next)
        {
            Next = next;
        }

        public IChain Next { get; set; }

        public double Calculate(Numbers numbers)
        {
            if (numbers.CalcOperator == CalcOperator.Add)
                return numbers.Number1 + numbers.Number2;
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
