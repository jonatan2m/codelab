using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.DynamicProgramming
{
    public class ClimbingStairs
    {
        //Return 0, if impossible to get the top and 1 if it is possible
        //O(2^n)
        public int CalculeTheWaysToClimbRecursive(int totalSteps, int currentSteps = 0)
        {
            //Fix the ways, 1 step or 2 steps
            if (currentSteps > totalSteps) { return 0; }
            if (currentSteps == totalSteps) return 1;

            return CalculeTheWaysToClimbRecursive(totalSteps, currentSteps + 1) + CalculeTheWaysToClimbRecursive(totalSteps, currentSteps + 2);
        }

        //Return 0, if impossible to get the top and 1 if it is possible
        //O(n)
        //Top-Down Dynamic Programming (It starts from the )
        public int CalculeTheWaysToClimbRecursiveAndMemoization(int totalSteps, Dictionary<int, int>? memo = null, int currentSteps = 0)
        {
            memo ??= new Dictionary<int, int>();

            //Fix the ways, 1 step or 2 steps
            if (currentSteps > totalSteps) { return 0; }
            if (currentSteps == totalSteps) return 1;

            if (memo.ContainsKey(currentSteps)) { return memo[currentSteps]; }

            var result = CalculeTheWaysToClimbRecursiveAndMemoization(totalSteps, memo, currentSteps + 1)
             + CalculeTheWaysToClimbRecursiveAndMemoization(totalSteps, memo, currentSteps + 2);

            memo.Add(currentSteps, result);

            return result;
        }

        //https://www.youtube.com/watch?v=Y0lT9Fck7qI
        //Bottom-up dynamic programming (improved)
        //It starts from the base case. In this problem, the base case is the top of the stairs.
        public int CalculeWithDifferentApproach(int totalSteps)
        {
            int a = 1;
            int b = 1;
            int c = 0;
            for (int i = totalSteps - 2; i >= 0; i--)
            {
                c = a + b;
                a = b;
                b = c;
            }

            return c;
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(5, 8)]
        [InlineData(15, 987)]
        public void SelfTest(int input, int expected)
        {
            //Arrange
            var sut = new ClimbingStairs();

            //Act
            var result = sut.CalculeTheWaysToClimbRecursive(input);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        //[InlineData(2, 2)]
        //[InlineData(3, 3)]
        [InlineData(5, 8)]
        //[InlineData(15, 987)]
        public void SelfTest2(int input, int expected)
        {
            //Arrange
            var sut = new ClimbingStairs();

            //Act
            var result = sut.CalculeTheWaysToClimbRecursiveAndMemoization(input);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(5, 8)]
        [InlineData(15, 987)]
        public void SelfTest3(int input, int expected)
        {
            //Arrange
            var sut = new ClimbingStairs();

            //Act
            var result = sut.CalculeWithDifferentApproach(input);

            //Assert
            Assert.Equal(expected, result);
        }

    }
}
