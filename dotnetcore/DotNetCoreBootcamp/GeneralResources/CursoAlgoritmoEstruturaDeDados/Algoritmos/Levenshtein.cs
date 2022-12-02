using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.Algoritmos
{
    /// <summary>
    /// The Levenshtein Algorithm
    /// https://medium.com/@ethannam/understanding-the-levenshtein-distance-equation-for-beginners-c4285a5604f0
    /// https://eximia.co/computing-the-levenshtein-edit-distance-of-two-strings-using-c/
    /// 
    /// </summary>
    public class Levenshtein
    {
        /// <summary>
        /// O(n²)
        /// </summary>
        public int Version1(string a, string b)
        {
            if (a.Length == 0) return b.Length;
            if (b.Length == 0) return a.Length;

            var matrix = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (int j = 0; j <= b.Length; j++)
            {
                matrix[0, j] = j;
            }

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    var match = matrix[i - 1, j - 1] + cost;
                    var insert = matrix[i, j - 1] + 1;
                    var delete = matrix[i - 1, j] + 1;
                    matrix[i, j] = Min(delete, insert, match);
                }
            }

            return matrix[a.Length, b.Length];
        }

        /// <summary>
        /// Otimizado. Economizando memória.
        /// Feito pelo Elemar, não consegui entender a lógica completa
        /// </summary>
        public int Version2(string a, string b)
        {
            if (a.Length == 0) return b.Length;
            if (b.Length == 0) return a.Length;

            var current = 1;
            var previous = 0;
            var matrix = new int[2, b.Length + 1];

            for (int i = 0; i <= b.Length; i++)
            {
                matrix[previous, i] = i;
            }

            for (int i = 0; i < a.Length; i++)
            {
                matrix[current, 0] = i + 1;

                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = b[j - 1] == a[i] ? 0 : 1;
                    matrix[current, j] = Min(
                        matrix[previous, j] + 1,
                        matrix[current, j - 1] + 1,
                        matrix[previous, j - 1] + cost);
                }
                previous = (previous + 1) % 2;
                current = (current + 1) % 2;
            }

            return matrix[previous, b.Length];
        }

        public int Min(int x, int y, int z)
        {
            return Math.Min(Math.Min(x, y), z);
        }

        [Theory]
        [InlineData("jonatan", "natan", 2)]
        [InlineData("ant", "aunt", 1)]
        [InlineData("a", "a", 0)]
        public void TestVersion1(string a, string b, int expected)
        {
            // Arrange
            var sut = new Levenshtein();

            // Act
            var result = sut.Version1(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("jonatan", "natan", 2)]
        [InlineData("ant", "aunt", 1)]
        [InlineData("a", "a", 0)]
        public void TestVersion2(string a, string b, int expected)
        {
            // Arrange
            var sut = new Levenshtein();

            // Act
            var result = sut.Version2(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }


}
