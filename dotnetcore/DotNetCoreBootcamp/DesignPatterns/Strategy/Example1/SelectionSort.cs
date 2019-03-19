using System;
namespace DesignPatterns.Strategy.Example1
{
    public class SelectionSort : ISort
    {
        public int[] Sort(int[] input)
        {
            var count = 0;
            var smallValueIndex = 0;

            while (count < input.Length)
            {
                for (int i = count + 1; i < input.Length; i++)
                {
                    if (input[smallValueIndex] > input[i])
                    {
                        smallValueIndex = i;
                    }
                }
                if (smallValueIndex != count)
                {
                    input[count] += input[smallValueIndex];
                    input[smallValueIndex] = input[count] - input[smallValueIndex];
                    input[count] = input[count] - input[smallValueIndex];
                }
                count++;
                smallValueIndex = count;
            }
            return input;
        }
    }
}
