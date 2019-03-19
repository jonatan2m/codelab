using System;
namespace DesignPatterns.Strategy.Example1
{
    public class SortingExample
    {
        ISort strategy;

        public SortingExample(ISort strategy)
        {
            ChangeStrategy(strategy);
        }

        public void ChangeStrategy(ISort strategy)
        {
            this.strategy = strategy;
        }

        public int[] Sort(int[] data)
        {
            return this.strategy.Sort(data);
        }
    }
}
