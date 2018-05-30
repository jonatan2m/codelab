using System;

namespace RecursiveLab
{
    internal class Fibonacci
    {
        internal int Calculate(int number)
        {
            if (number == 0 || number == 1)
                return number;

            return Calculate(number - 1) + Calculate(number - 2);
        }

        internal double CalculateByClosedFormula(int number)
        {
            double part1 = Math.Pow((1 + Math.Sqrt(5)) / 2, number);
            double part2 = Math.Pow((1 - Math.Sqrt(5)) / 2, number);
            double fn = (part1 - part2) / Math.Sqrt(5);

            return Math.Round(fn);
        }
    }
}