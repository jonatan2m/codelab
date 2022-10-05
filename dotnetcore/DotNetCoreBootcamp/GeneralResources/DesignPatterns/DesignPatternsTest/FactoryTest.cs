using DesignPatterns.Factory.AbstractFactory;
using DesignPatterns.Factory.EncapsulateClasses;
using DesignPatterns.Factory.Example1;
using DesignPatterns.Factory.LoanExample;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DesignPatternsTest
{
    public class FactoryTest
    {
        [Fact]
        public void Factory_Example1()
        {
            IBuilding house = DesignPatterns.Factory.Example1.BuildingFactory.GetInstanceOf("house");
            Assert.IsType<House>(house);
        }

        [Fact]
        public void AbstractFactory_Example1()
        {
            PizzaStore pizzaStore = new NYPizzaStore();
            pizzaStore.OrderPizza("cheese");
            Assert.IsAssignableFrom<PizzaStore>(pizzaStore);
        }

        private readonly int RISK_RATING = 1;

        [Fact]
        public void Loan_Factory_Example()
        {
            //arrange
            DateTime maturity = DateTime.Now.AddDays(5);
            DateTime? startOfLoan = DateTime.Now.AddYears(-2);

            Loan termLoan = LoanFactory.newTermLoan(10000.00, startOfLoan, maturity, RISK_RATING);
            termLoan.setOutstanding(10000.00);

            //act
            var result = termLoan.calcCapital();

            //assert
            Assert.Equal(20136, (int)result);
        }

        [Fact]
        public void EncapsulateClasses_AttributeDescriptor()
        {
            //arrange
            var result = new List<AttributeDescriptor>();

            //Act
            result.Add(AttributeDescriptor.ForInt("remoteId"));
            result.Add(AttributeDescriptor.ForDatetime("createdDate"));
            result.Add(AttributeDescriptor.ForBool("isBuyer"));

            //assert
            Assert.Equal("remoteId", result[0].GetName());
        }
    }
}
