namespace algoritmos.tests;

/// <summary>
/// https://leetcode.com/problems/maximum-average-subarray-i/description/
/// https://dotnetfiddle.net/Esxfv0
/// 
/// takeaways:
/// Consider all numbers as negative numbers (compute first kth elements)
/// 
/// Test case:
/// nums = [1,12,-5,-6,50,3]
/// k = 4
/// max average = 12.75
/// </summary>
public class leet643
{

    [Fact]
    public void Leet643()
    {
        int[] nums = new[] { 1, 12, -5, -6, 50, 3 };
        int k = 4;
        double result = 12.75;

        Assert.Equal(result, FindMaxAverageV1(nums, k));
        Assert.Equal(result, FindMaxAverageV2(nums, k));
        Assert.Equal(result, FindMaxAverageV3(nums, k));
    }

    /// <summary>
    /// Brute force optimized
    /// </summary>            
    public double FindMaxAverageV1(int[] nums, int k)
    {
        double sum = 0;
        for (int i = 0; i < k; i++) sum += nums[i];
        double maxAverage = sum / k;

        int index = 1;

        while (nums.Length - index >= k)
        {
            sum = 0;
            int end = index + k;
            for (int i = index; i < end; i++)
            {
                sum += nums[i];
            }
            double currentAverage = sum / k;
            maxAverage = maxAverage < currentAverage ? currentAverage : maxAverage;
            index++;
        }

        return maxAverage;
    }

    public double FindMaxAverageV2(int[] nums, int k)
    {
        double sum = 0;
        for (int i = 0; i < k; i++) sum += nums[i];
        double maxAverage = sum / k;

        for (int i = k; i < nums.Length; i++)
        {
            sum += nums[i] - nums[i - k];
            maxAverage = Math.Max(maxAverage, sum / k);
        }

        return maxAverage;
    }

    public double FindMaxAverageV3(int[] nums, int k)
    {
        long maxSum = 0;
        long currentSum = 0;
        for (int i = 0; i < k; i++) currentSum += nums[i];
        maxSum = currentSum;

        for (int i = k; i < nums.Length; i++)
        {
            currentSum += nums[i] - nums[i - k];
            if (currentSum > maxSum) maxSum = currentSum;
        }

        return (double)maxSum / k;
    }

}



    

