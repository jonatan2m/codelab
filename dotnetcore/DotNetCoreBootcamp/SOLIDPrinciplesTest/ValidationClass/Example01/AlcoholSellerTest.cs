using SOLIDPrinciples.ValidationClass.Example01;
using SOLIDPrinciples.ValidationClass.Example01.BestSolution;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example01
{
    public class AlcoholSellerTest
    {
        [Fact]
        public void ShouldSellDrinksToPersonWhenAgeIsMoreThan18()
        {
            //Arrange
            Person person = new Person
            {
                Age = 20,
                ConsumesAlcohol = true,
                Name = "John"
            };

            AssertHelper(person, true);
        }

        private void AssertHelper(Person person, bool expected)
        {
            ValidationList validations = new ValidationList();
            validations.Add(new Age0OrHigherValidation(person));
            validations.Add(new OnlyAdultsCanConsumeAlcoholValidation(person));

            AlcoholSeller alcoholSeller = new AlcoholSeller(validations);

            //Act
            var actual = alcoholSeller.IsValid();

            Assert.Equal(expected, actual);
        }
    }
}
