using DomainEvents.NotifyPropertyChangedExample;
using System;
using Xunit;
using Xunit.Abstractions;

namespace DomainEventsTest.NotifyPropertyChangedExample
{
    public class StudentTest
    {
        private readonly ITestOutputHelper _output;

        public StudentTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        [Trait("Category", "Property")]
        public void CreateStudentAndChangeGradeProperty()
        {
            var sut = new Student("John", 123456);

            sut.PropertyChanged += (obj, arg) => _output.WriteLine(arg.PropertyName); ;

            sut.Grade = 10;
        }

        [Fact]
        [Trait("Category", "Property")]
        public void CreateStudentAndVerifyIfGradePropertyIsChange()
        {
            var sut = new Student("John", 123456);

            Assert.PropertyChanged(sut, nameof(sut.Grade), () => sut.UpdateGrade(10));
        }        
    }
}
