using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecursiveLab
{
    public class PermutationStringTest
    {
        [Theory]
        [InlineData("a", new string[] { "a" })]
        [InlineData("", new string[] { "" })]
        [InlineData("ab", new string[] { "ab", "ba" })]
        [InlineData("abc", new string[] { "abc", "acb", "bac", "bca", "cab", "cba" })]
        public void Should_return_the_string_permutation(string text, string[] expected)
        {
            var permutationString = new PermutationString();

             string[] result = permutationString.Permute("", text);

            Assert.Equal(expected, result);

        }
    }
}
