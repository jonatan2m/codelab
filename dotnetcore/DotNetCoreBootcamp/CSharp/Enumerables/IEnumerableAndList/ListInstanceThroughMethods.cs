using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Enumerables.IEnumerableAndList
{
    public class Sheet
    {
        public int Page { get; set; }
        private bool _clean, _scribble, _text;

        public Sheet()
        {
            var seed = Guid.NewGuid().GetHashCode();
            var aleatory = new Random(seed).Next(1, 4);
            _clean = aleatory == 1;
            _scribble = aleatory == 2;
            _text = aleatory == 3;
        }

        public bool HasScribble()
        {
            Console.WriteLine($"Looking for Scribble on Page: {Page}");
            return _scribble;
        }
    }

    public class ListInstanceThroughMethods
    {
        public void Scenario1()
        {
            Console.WriteLine(nameof(Scenario1));
            var ids = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            PrintNumers(ids);
        }

        /// <summary>
        /// Nothing happens
        /// </summary>
        public void Scenario2()
        {
            Console.WriteLine(nameof(Scenario2));
            var sheets = new List<Sheet>();

            for (int i = 1; i <= 10; i++)
            {
                sheets.Add(new Sheet { Page = i });
            }

            var sheetsWithScribble = sheets.Select(x => x.HasScribble());
        }

        /// <summary>
        /// To List materialize all collection
        /// </summary>
        public void Scenario3()
        {
            Console.WriteLine(nameof(Scenario3));
            var sheets = new List<Sheet>();

            for (int i = 1; i <= 10; i++)
            {
                sheets.Add(new Sheet { Page = i });
            }

            var sheetsWithScribble = sheets.Select(x => x.HasScribble()).ToList();
        }

        /// <summary>
        /// IEnumerable multiple enumeration
        /// </summary>
        public void Scenario4()
        {
            Console.WriteLine(nameof(Scenario4));
            var sheets = new List<Sheet>();

            for (int i = 1; i <= 10; i++)
            {
                sheets.Add(new Sheet { Page = i });
            }

            var sheetsWithScribble = sheets.Select(x => x.HasScribble());

            if (sheetsWithScribble.Count() > 1) { /*some processing*/ }


            if (sheetsWithScribble.Count() > 5) { /*some processing*/ }
        }

        /// <summary>
        /// IEnumerable better usage
        /// </summary>
        public void Scenario5()
        {
            Console.WriteLine(nameof(Scenario5));
            var sheets = new List<Sheet>();

            for (int i = 1; i <= 10; i++)
            {
                sheets.Add(new Sheet { Page = i });
            }

            var sheetsWithScribble = sheets.Select(x => x.HasScribble());

            var firstSheet = sheetsWithScribble.FirstOrDefault();

            Console.WriteLine("Another example filtering on LINQ method");
            _ = sheets.FirstOrDefault(x => x.HasScribble());
        }

        private void PrintNumers(IEnumerable<int> numbers)
        {
            if (numbers is List<int>)
                Console.WriteLine("it is a List<int>");

            numbers.Select(x => { Console.WriteLine(x); return x; });
        }
    }
}
