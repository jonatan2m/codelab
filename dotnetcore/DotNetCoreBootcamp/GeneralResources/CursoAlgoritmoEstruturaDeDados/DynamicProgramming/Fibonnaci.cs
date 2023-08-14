using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.DynamicProgramming
{
    public class Fibonnaci
    {
        [Fact]
        public void TestFiboRecursive()
        {
            var result = FiboRecursive(5);

            Assert.Equal(5, result);
        }

        [Fact]
        public void TestFiboIterative()
        {
            var result = FiboIterative(5);

            Assert.Equal(5, result);
        }

        //O(n) memory and time
        private long FiboRecursive(int nth, long[]? d = null)
        {
            if(nth == 0 || nth == 1)
            {
                return nth;
            }

            d ??= new long[nth + 1];

            if (d[nth] == 0)
            {
                d[nth] = FiboRecursive(nth - 1, d) + FiboRecursive(nth - 2, d);
            }

            return d[nth];
        }

        //O(n) time, O(1) memory 
        private long FiboIterative(int nth)
        {
            if (nth == 0 || nth == 1)
            {
                return nth;
            }

            long a = 0; long b = 1; long c = 1;

            for (int i = 2; i <= nth; i++)
            {
                c = b + a;
                a = b;
                b = c;
            }

            return c;
        }
    }
}
