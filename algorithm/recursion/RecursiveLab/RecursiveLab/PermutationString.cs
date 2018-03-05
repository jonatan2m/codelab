using System;
using System.Collections.Generic;
using System.Text;

namespace RecursiveLab
{
    internal class PermutationString
    {
        internal string[] Permute(string beginningString, string endingString)
        {
            if (endingString.Length <= 1)
                return CombineStringToArray(beginningString, endingString);
            else
            {
                var result = new List<string>();
                for (int i = 0; i < endingString.Length; i++)
                {
                    var newString = RemoveCharAt(endingString, i);

                    result.AddRange(
                        Permute($"{beginningString}{endingString[i]}", newString)
                        );
                }

                return result.ToArray();
            }
        }

        private string[] CombineStringToArray(string beginningString, string endingString)
        {
            return new string[] { $"{beginningString}{endingString}" };
        }

        private string RemoveCharAt(string text, int index)
        {
            var part1 = text.Substring(0, index);
            var part2 = text.Substring(index + 1);

            return $"{part1}{part2}";
        }
    }
}