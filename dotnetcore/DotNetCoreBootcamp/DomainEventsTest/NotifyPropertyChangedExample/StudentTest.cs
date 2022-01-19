using DomainEvents.NotifyPropertyChangedExample;
using System;
using Xunit;

namespace DomainEventsTest.NotifyPropertyChangedExample
{
    public class StudentTest
    {
        [Fact]
        public void CreateStudentAndChangeGradeProperty()
        {
            var sut = new Student("John", 123456);

            sut.PropertyChanged += (obj, arg) => Console.WriteLine(arg.PropertyName); ;

            sut.Grade = 10;
        }

        [Fact]
        public void CreateStudentAndVerifyIfGradePropertyIsChange()
        {
            var sut = new Student("John", 123456);

            Assert.PropertyChanged(sut, nameof(sut.Grade), () => sut.UpdateGrade(10));
        }        
    }
}
