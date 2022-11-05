using System.Collections;

namespace GeneralResources.XUnitExample.WorkingWithData
{
    public class Person1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class TheoryDataExamples02
    {
        public static IEnumerable<object[]> GetNumbersFromDataGenerator()
        {
            yield return new object[] { 5, 1, 3, 9 };
            yield return new object[] { 7, 1, 5, 3 };
        }

        public static TheoryData<Person1> PersonData => new TheoryData<Person1>
        {
            new Person1 {Name = "Tribbiani", Age = 56},
            new Person1 {Name = "Gotti", Age = 16},
            new Person1 {Name = "Sopranos", Age = 15},
            new Person1 {Name = "Corleone", Age = 27},
            new Person1 {Name = "Mancini", Age = 79},
            new Person1 {Name = "Vivaldi", Age = 16},
            new Person1 {Name = "Serpico", Age = 19},
            new Person1 {Name = "Salieri", Age = 20}
        };
    }

    public class ParameterizedTheoryDataTests
    {
        public bool IsAboveFourteen(Person1 person)
        {
            return person.Age > 14;
        }

        [Theory]
        [MemberData(nameof(TheoryDataExamples02.PersonData), MemberType = typeof(TheoryDataExamples02))]
        public void AllPersons_AreAbove14_WithMemberData_FromDataGenerator(Person1 a)
        {
            Assert.True(IsAboveFourteen(a));            
        }
    }
}
