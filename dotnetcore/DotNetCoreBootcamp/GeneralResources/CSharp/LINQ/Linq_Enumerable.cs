using Castle.Components.DictionaryAdapter;
using DomainEvents.MediatRExample1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

using static CSharp.Enumerables.YieldReturn.Example02;

namespace GeneralResources.CSharp.LINQ
{

    /// <summary>
    /// LINQ operators being applied to IEnumerable collections
    /// Filtering (Where, OfType)
    /// Projection(Select, SelectMany)
    /// Partitioning(Skip, Take)
    /// Ordering(OrderBy, ThenBy, Reverse)
    /// Grouping(GroupBy, ToLookup)
    /// Set operations(mentioned previously)
    /// Conversion(ToArray, ToDictionary, OfType, Cast)
    /// Element(First, Last, Single, ElementAt)
    /// Aggregation(also mentioned previously)
    /// 
    /// Generation operators, like Range, Repeat, and Empty, 
    /// create new instances of collections with specific characteristics.
    /// 
    /// Take(n): Retrieves the first n elements
    /// Skip(n) : Skips the first n elements and returns the remaining elements
    /// TakeWhile(condition) : Takes elements while a certain condition holds true
    /// SkipWhile(condition) : Skips elements while a condition holds true and returns the remaining elements
    /// 
    /// System.Data.Linq for LinQ to SQL
    /// System.Data.Entity for Entity Framework
    /// System.Xml.Linq for LinQ to XML
    /// 
    /// </summary>
    public class Linq_Enumerable
    {
        [Fact]
        public void Using_IEnumerable_Examples()
        {
            List<string> names = new List<string> { "Alice", "Bob", "Carol", "Carla", "David" };

            IEnumerable<string> namesStartingWithC = names;


            namesStartingWithC = namesStartingWithC.Where(StartsWithC);

            namesStartingWithC = namesStartingWithC.OrderBy(OrderByName);

            var result = namesStartingWithC.ToList();

            Assert.Equal(2, result.Count);
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
        public void Using_TPL_to_process_a_long_list()
        {
            var numbers = Enumerable.Range(1, 1000000);  // Generates numbers 1 to 10

            Stopwatch stopwatch= Stopwatch.StartNew();
            var r = numbers.Where(FakeCalculation).ToList();
            stopwatch.Stop();
            long result1 = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            //In some cases, using the .AsOrdered() extension method can help maintain the order of the elements,
            //especially in situations that require it.
            var t = numbers.AsParallel()
                .Where(FakeCalculation).ToList();
            stopwatch.Stop();
            long result2 = stopwatch.ElapsedMilliseconds;
        }

        private bool FakeCalculation(int num)
        {
            return (num * new Random(1000).NextInt64()) % 2 == 0;
        }

        [Fact]
        public void Example02()
        {
            // Generation operator example - Range
            var numbers = Enumerable.Range(1, 10);  // Generates numbers 1 to 10

            // Generation operator example - Repeat
            var repeatedValue = Enumerable.Repeat("Hello", 5); // Creates an IEnumerable with 5 "Hello" values

            // Pagination example
            int pageNumber = 1;
            int pageSize = 5;
            
            List<string> students = new List<string> { "Alice", "Bob", "Carol", "Carla", "David" };

            var page = students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Pagination example - second page
            pageNumber = 2;

            var secondPage = students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
