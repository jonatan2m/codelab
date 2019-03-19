using System;
using DesignPatterns.Strategy.Example1;
using Xunit;

namespace DesignPatternsTest
{
    public class StrategyTest
    {
        [Fact]
        public void Strategy_Example1()
        {

            var sorting = new SortingExample(new BubleSort());
            var result = sorting.Sort(GetDummyData());

            sorting.ChangeStrategy(new SelectionSort());
            sorting.Sort(GetDummyData());

            Assert.Equal(new int[] { 1, 2, 5, 7 }, result);
        }

        public int[] GetDummyData()
        {
            return new int[] { 5, 2, 7, 1 };

        }
    }
}
