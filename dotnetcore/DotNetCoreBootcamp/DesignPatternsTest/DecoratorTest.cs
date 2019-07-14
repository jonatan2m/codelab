using System;
using System.Collections.Generic;
using System.Text;
using Example1 = DesignPatterns.Decorator.CarRentalExample;
using Example2 = DesignPatterns.Decorator.CarRentalExample2;
using Xunit;

namespace DesignPatternsTest
{
    public class DecoratorTest
    {
        [Fact]
        public void CanRentalWithNoneOfSpecialOptions()
        {
            Example1.Model m = new Example1.Model(10.0f, 50.0f, "Ford Taurus");
            Example1.IRental r1 = new Example1.CarRental(m, 5);
            Assert.True(r1.CalcPrice() == 250.0f);
        }

        [Fact]
        public void CanRentalWithInsuranceOption()
        {
            Example1.Model m = new Example1.Model(10.0f, 50.0f, "Ford Taurus");
            Example1.IRental r1 = new Example1.Insurance(new Example1.CarRental(m, 5), 12.5f);
            Assert.True(r1.CalcPrice() == 312.5f);
        }

        [Fact]
        public void CanRentalWithFuelAndInsuranceOptions()
        {
            Example1.Model m = new Example1.Model(10.0f, 50.0f, "Ford Taurus");
            Example1.IRental r1 = new Example1.RefuelOnReturn(
                new Example1.Insurance(new Example1.CarRental(m, 5), 12.5f), 3.75f);
            Assert.True(r1.CalcPrice() == 350.0f);
        }

        [Fact]
        public void CanRentalWithNoneOfSpecialOptions1()
        {
            Example2.Model m = new Example2.Model(10.0f, 50.0f, "Ford Taurus");
            Example2.IRental r1 = new Example2.CarRental(m, 5);
            Assert.True(r1.calcPrice() == 250.0f);
        }

        [Fact]
        public void CanRentalWithInsuranceOption1()
        {
            Example2.Model m = new Example2.Model(10.0f, 50.0f, "Ford Taurus");
            Example2.IRental r2 = new Example2.Insurance(
                new Example2.CarRental(m, 5),
                12.5f);

            Assert.True(r2.calcPrice() == 312.5f);
        }

        [Fact]
        public void CanRentalWithFuelAndInsuranceOptions1()
        {
            Example2.Model m = new Example2.Model(10.0f, 50.0f, "Ford Taurus");
            Example2.IRental r3 = new Example2.RefuelOnReturn(
                new Example2.Insurance(
                    new Example2.CarRental(m, 5), 12.5f), 3.75f);
            
            Assert.True(r3.calcPrice() == 350.0f);
        }
    }
}
