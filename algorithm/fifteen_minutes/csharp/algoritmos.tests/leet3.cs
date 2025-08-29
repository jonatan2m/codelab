namespace algoritmos.tests;

/// <summary>
/// https://leetcode.com/problems/longest-substring-without-repeating-characters/
/// 
/// takeaways:
/// Consider all numbers as negative numbers (compute first kth elements)
/// 
/// Test case:
/// nums = [1,12,-5,-6,50,3]
/// k = 4
/// max average = 12.75
/// </summary>
public class leet3
{

    [Theory]
    [InlineData("abcabcbb", 3)]
    [InlineData("bbbbb", 1)]
    [InlineData("b", 1)]
    [InlineData("au", 2)]
    [InlineData("pwwkew", 3)]
    public void Leet3(string input, int result)
    {
        Assert.Equal(result, LengthOfLongestSubstringV1(input));
        Assert.Equal(result, LengthOfLongestSubstringV2(input));
        Assert.Equal(result, LengthOfLongestSubstringV3(input));
    }

    /// <summary>
    /// Brute force
    /// </summary>
    public int LengthOfLongestSubstringV1(string s)
    {
        Dictionary<char, int> seq = new();
        int maxSeq = 0;

        for (int i = 0; i < s.Length - 1; i++)
        {
            seq.Add(s[i], i);

            for (int j = i + 1; j < s.Length; j++)
            {
                if (seq.ContainsKey(s[j])) break;

                seq.Add(s[j], j);
            }
            if (seq.Count > maxSeq) maxSeq = seq.Count;
            seq.Clear();
        }

        return maxSeq;
    }

    public int LengthOfLongestSubstringV2(string s)
    {
        if (s.Length == 0) return 0;

        int start = 0;
        int end = 0;
        int max = 1;
        Dictionary<char, int> seq = new();
        seq.Add(s[start], start);
        end++;

        while (start <= end && end < s.Length)
        {
            if (seq.ContainsKey(s[end]) is false)
            {
                seq.Add(s[end], end);
                end++;
            }
            else
            {
                if (max < end - start) max = end - start;

                seq.Remove(s[start]);
                start++;
            }
        }

        if (max < end - start) max = end - start;

        return max;
    }

    /// <summary>
    /// By Copilot
    /// </summary>
    public int LengthOfLongestSubstringV3(string s)
    {
        if (s.Length == 0) return 0;

        int start = 0;
        int max = 1;
        Dictionary<char, int> lastSeen = new();

        for (int end = 0; end < s.Length; end++)
        {
            if (lastSeen.ContainsKey(s[end]) && lastSeen[s[end]] >= start)
            {
                start = lastSeen[s[end]] + 1;
            }
            lastSeen[s[end]] = end;
            max = Math.Max(max, end - start + 1);
        }

        return max;
    }
}