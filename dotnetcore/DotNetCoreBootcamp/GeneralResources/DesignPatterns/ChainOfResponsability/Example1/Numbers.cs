using System;
namespace DesignPatterns.ChainOfResponsability.Example1
{

    public class Numbers
    {
        public int Number1 { get; private set; }
        public int Number2 { get; private set; }

        public CalcOperator CalcOperator { get; private set; }

        public Numbers(int number1, int number2, CalcOperator calcOperator)
        {
            Number1 = number1;
            Number2 = number2;
            CalcOperator = calcOperator;
        }
    }
}
