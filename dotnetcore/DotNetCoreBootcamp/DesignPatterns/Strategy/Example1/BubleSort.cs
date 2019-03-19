using System;
namespace DesignPatterns.Strategy.Example1
{
    public class BubleSort : ISort
    {
        public int[] Sort(int[] input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    if (input[i] > input[j])
                    {
                        input[i] += input[j];
                        input[j] = input[i] - input[j];
                        input[i] = input[i] - input[j];
                    }
                }
            }

            return input;
        }
    }
}
