namespace algoritmos.tests;

/// <summary>
/// https://leetcode.com/problems/longest-substring-without-repeating-characters/
/// 
/// takeaways:
/// "Some edge cases can break the algorithm, and working with dynamic windows is quite hard."
/// 
/// </summary>
public class leet3
{

    [Theory]
    [InlineData("abcabcbb", 3)]
    [InlineData("bbbbb", 1)]
    [InlineData("b", 1)]
    [InlineData("au", 2)]
    [InlineData("pwwkew", 3)]
    [InlineData("dvdp", 3)]
    [InlineData("abcb", 3)]
    public void Leet3(string input, int result)
    {
        Assert.Equal(result, LengthOfLongestSubstringV1(input));
        Assert.Equal(result, LengthOfLongestSubstringV2(input));
        Assert.Equal(result, LengthOfLongestSubstringV3(input));
        Assert.Equal(result, LengthOfLongestSubstringV4(input));
        Assert.Equal(result, LengthOfLongestSubstringV5(input));
    }

    /// <summary>
    /// Brute force
    /// </summary>
    public int LengthOfLongestSubstringV1(string s)
    {
        Dictionary<char, int> seq = new();
        int maxSeq = 0;

        for (int i = 0; i < s.Length; i++)
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

    /// <summary>
    /// Mixed
    /// </summary>
    public int LengthOfLongestSubstringV4(string s)
    {
        if (s.Length == 0) return 0;

        int start = 0;
        int max = 0;
        Dictionary<char, int> lastest = new();

        for (int end = 0; end < s.Length; end++)
        {
            char c = s[end];
            if (lastest.ContainsKey(c))
            {
                start = int.Max(start, lastest[c] + 1);
            }

            lastest[c] = end;

            max = int.Max(max, (end - start) + 1);
        }

        return max;
    }
    
     public int LengthOfLongestSubstringV5(string s) {
        int maxLength = 0;
        int left = 0;
        var lastSeen = new Dictionary<char, int>();

        for (int right = 0; right < s.Length; right++) {
            char c = s[right];

            if (lastSeen.ContainsKey(c) && lastSeen[c] >= left) {
                left = lastSeen[c] + 1;
            }

            maxLength = Math.Max(maxLength, right - left + 1);
            lastSeen[c] = right;
        }

        return maxLength;
    }
}

/* That's simple and interesting solution in Python
class Solution:
    def lengthOfLongestSubstring(self, s: str) -> int:
        left = max_length = 0
        char_set = set()
        
        for right in range(len(s)):
            while s[right] in char_set:
                char_set.remove(s[left])
                left += 1

            char_set.add(s[right])
            max_length = max(max_length, right - left + 1)
        
        return max_length
*/