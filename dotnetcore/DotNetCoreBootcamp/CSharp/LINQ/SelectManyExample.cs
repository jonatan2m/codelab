using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharp.LINQ
{
    /// <summary>
    /// https://stackoverflow.com/questions/958949/difference-between-select-and-selectmany
    /// </summary>
    public class SelectManyExample
    {
        public class Person
        {
            public int Id { get; set; }
            public List<PhoneNumber> PhoneNumbers { get; set; }
        }

        public class PhoneNumber
        {
            public int Number { get; set; }
        }

        private readonly List<Person> People;

        public SelectManyExample()
        {
            People = new List<Person>();

            for (int i = 0; i < 5; i++)
            {
                var person = new Person();
                person.Id = i;
                person.PhoneNumbers = new List<PhoneNumber>();
                for (int j = 0; j < 5; j++)
                {
                    person.PhoneNumbers.Add(new PhoneNumber()
                    {
                        Number = j
                    });
                }

                People.Add(person);
            }
        }

        public void SelectMany01()
        {
            var time = Stopwatch.StartNew();
            var result = People.SelectMany(p =>
            {
                return p.PhoneNumbers.Select(y =>
                {
                    Console.Write($"{y.Number}, ");
                    return new { Code = y.Number };
                });
            }).ToList();

            time.Stop();

            Console.WriteLine($"People.SelectMany + PhoneNumbers.Select {time.ElapsedTicks}");

            time.Restart();
            result = People.SelectMany(p => p.PhoneNumbers, (person, phone) =>
            {
                Console.Write($"{phone.Number}, ");
                return new {Code = phone.Number};
            }).ToList();
            time.Stop();

            Console.WriteLine($"People.SelectMany (person, phone) {time.ElapsedTicks}");
        }
    }
}
