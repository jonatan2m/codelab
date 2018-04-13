using System;
using Xunit;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodingInterview
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(new int[] { 2, 3, 4, 5, 6, 1 }, "2 3 4 5 6 1", 1)]
        [InlineData(new int[] { 3, 4, 5, 6, 1, 2 }, "3 4 5 6 1 2", 2)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, "1 2 3 4 5 6", 6)]
        public void should_left_rotate(int[] expected, string stringExpected, int times)
        {
            var arrExample = new int[] { 1, 2, 3, 4, 5, 6 };
            LeftRotation left = new LeftRotation(arrExample);

            left.Rotate(times);
            var stringResult = left.Print();
            

            Assert.Equal(expected, left.Array());

            Assert.Equal(stringExpected, stringResult);
        }

        [Theory]
        [InlineData("2 3 4 5 6 1", 1)]
        [InlineData("3 4 5 6 1 2", 2)]
        [InlineData("1 2 3 4 5 6", 6)]
        public void should_left_rotate_trick(string expected, int times)
        {
            var arrExample = new int[] { 1, 2, 3, 4, 5, 6 };
            LeftRotation left = new LeftRotation(arrExample);

            var stringResult = left.RotateTrick(times);
                        
            Assert.Equal(expected, stringResult);
        }

        [Theory]
        [InlineData(new int[] { 2, 3, 4, 5, 6, 1 }, "2 3 4 5 6 1", 1)]
        [InlineData(new int[] { 3, 4, 5, 6, 1, 2 }, "3 4 5 6 1 2", 2)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, "1 2 3 4 5 6", 6)]
        public void should_left_rotate_dotnet(int[] expected, string stringExpected, int times)
        {
            var arrExample = new int[] { 1, 2, 3, 4, 5, 6 };
            LeftRotation left = new LeftRotation(arrExample);

            left.RotateDotNet(times);
            var stringResult = left.Print();


            Assert.Equal(expected, left.Array());

            Assert.Equal(stringExpected, stringResult);
        }
    }
}
