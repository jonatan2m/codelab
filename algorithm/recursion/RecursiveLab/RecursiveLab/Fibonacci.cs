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
    }
}