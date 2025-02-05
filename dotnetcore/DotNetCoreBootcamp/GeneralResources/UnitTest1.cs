using GeneralResources.CursoAlgoritmoEstruturaDeDados.Sorting;
using System.Collections.Generic;

namespace GeneralResources
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<string> names = new List<string> { "Alice", "Bob", "Carol", "Carla", "David" };

            IEnumerable<string> namesStartingWithC = names;


            namesStartingWithC = namesStartingWithC.Where(StartsWithC);

            namesStartingWithC = namesStartingWithC.OrderBy(OrderByName);

            var result = namesStartingWithC.ToList();
        }

        private bool StartsWithC(string name)
        {
            return name.StartsWith("C");
        }

        private string OrderByName(string name)
        {
            return name;
        }

        [Fact]
        public void TesteHeap()
        {
            int a = 100;
            int b = 19;

            Assert.True(a.CompareTo(b) == 1);
        }
    }
}