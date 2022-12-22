using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.Algoritmos
{
    /// <summary>
    /// https://www.geeksforgeeks.org/0-1-knapsack-problem-dp-10/
    /// Dado uma lista de itens, onde cada 1 tem um peso e um valor,
    /// Descobrir qual a melhor combinação que cabe dentro de uma mochila
    /// que suporta W de peso.    
    /// 
    /// Example:
    /// Input: N = 3, W = 4
    /// values[] = {1,2,3}
    /// weight[] = { 4,5,1}
    /// Output: 3
    /// 
    /// Input: N = 3, W = 3
    /// values[] = { 1,2,3}
    /// weight[] = { 4,5,6}
    /// Output: 0
    /// 
    /// Número de combinações possives é 2^N
    /// 
    /// Um das formas de resolver seria ordernar por weight ratio (razão)
    /// e pegar a melhor delas, mas essa abordagem por não fazer a melhor escolha
    /// pois pode desconsiderar
    /// 
    /// 
    ///
    /// </summary>
    public class Knapsack
    {
        /// <summary>
        /// Recursive Top-Down O(2^n)
        /// </summary>        
        public static int Version1(int W, int[] wt, int[] val, int n)
        {
            if (W == 0 || n == 0) return 0;

            if (wt[n - 1] > W)
                return Version1(W, wt, val, n - 1);
            else
            {
                int num1 = val[n - 1] +
                    Version1(W - wt[n - 1], wt, val, n - 1);
                int num2 = Version1(W, wt, val, n - 1);
                return Math.Max(num1, num2);
            }
        }

        /// <summary>
        /// Recursive with Memoization Top-Down O(N * W)
        /// </summary>        
        public static int Version2(int W, int[] wt, int[] val, int n)
        {
            int[,] dp = new int[n + 1, W + 1];

            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < W + 1; j++)
                    dp[i, j] = -1;

            return Version2Memoization(W, wt, val, n, dp);
        }
               
        static int Version2Memoization(int W, int[] wt, int[] val, int n, int[,] dp)
        {
            if (W == 0 || n == 0) return 0;

            if (dp[n, W] != -1) return dp[n, W];

            if (wt[n - 1] > W)
                return dp[n, W] = Version2Memoization(W, wt, val, n - 1, dp);
            else
            {
                int num1 = val[n - 1] +
                    Version2Memoization(W - wt[n - 1], wt, val, n - 1, dp);
                int num2 = Version2Memoization(W, wt, val, n - 1, dp);
                return dp[n, W] = Math.Max(num1, num2);
                //return Math.Max(val[n - 1] +
                //    Version1(W - wt[n - 1], wt, val, n - 1),
                //    Version1(W, wt, val, n - 1));
            }
        }

        /// <summary>
        /// Use dynamic programming
        /// https://bitbucket.org/StableSort/play/src/master/js/01knapsack/01knapsack.js
        /// https://www.youtube.com/watch?v=-kedQt2UmnE
        /// </summary>
        /// <returns></returns>
        public static int Version3(int W_max, int[] wt, int[] val, int n)
        {
            var T = new int[n + 1, W_max + 1];
            var keep = new int[n + 1, W_max + 1];

            for (int i = 0; i <= W_max; i++) T[0, i] = 0;

            for (int i = 1; i <= n; i++)
                for (int w = 0; w <= W_max; w++)
                {
                    int itemValue = val[i - 1];
                    int itemWeight = wt[i - 1];

                    int diffWeight = w - itemWeight;
                    int newBest = itemValue + T[i - 1, diffWeight < 0 ? 0 : diffWeight];

                    if (wt[i - 1] <= w && T[i - 1, w] < newBest)
                    {
                        T[i, w] = newBest;
                        keep[i, w] = 1;
                    }
                    else
                    {
                        T[i, w] = T[i - 1, w];
                        keep[i, w] = 0;
                    }
                }
            
            int _w = W_max;
            List<int> items = new List<int>();
            for (int i = n; i >= 1; i--)
            {
                if (keep[i, _w] == 1)
                {
                    items.Add(val[i - 1]);
                    _w -= wt[i - 1];
                }
            }

            return items.Sum(x => x);
        }

        [Fact]
        public void TesteV1()
        {
            var values = new int[] { 1, 2, 3 };
            var weights = new int[] { 4, 5, 1 };
            int N = values.Length; int W = 4;
            int expectedOutput = 3;

            var result = Version1(W, weights, values, N);

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void TesteV2()
        {
            var values = new int[] { 1, 2, 3 };
            var weights = new int[] { 4, 5, 1 };
            int N = values.Length; int W = 4;
            int expectedOutput = 3;

            var result = Version2(W, weights, values, N);

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void TesteV3()
        {
            var values = new int[] { 1, 2, 3 };
            var weights = new int[] { 4, 5, 1 };
            int N = values.Length; int W = 4;
            int expectedOutput = 3;

            var result = Version3(W, weights, values, N);

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void TesteV3_1()
        {
            var values = new int[] { 10, 40, 60, 30 };
            var weights = new int[] { 5, 4, 3, 6 };
            int N = values.Length; int W = 10;
            int expectedOutput = 100;

            var result = Version3(W, weights, values, N);

            Assert.Equal(expectedOutput, result);
        }
    }
}
