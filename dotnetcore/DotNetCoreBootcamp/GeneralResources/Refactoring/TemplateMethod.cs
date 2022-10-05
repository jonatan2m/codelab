using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring
{
    /*
     * A ideia aqui é identificar comportamentos similares em classes e aplicar herança para que elas tenham seu código comum num só lugar
     * e o código especifico fica em cada uma das subclasses.
     */
    public class TemplateMethod
    {
        [Fact]
        public void TestBefore()
        {
            var residential = new ResidentialSiteBefore(1, 2);
            var amount = residential.GetBillableAmount();

            var lifeline = new LifelineSiteBefore(1, 2);
            var amount2 = lifeline.GetBillableAmount();

            Assert.True(amount > amount2);
        }

        [Fact]
        public void TestAfter()
        {
            Site residential = new ResidentialSite(1, 2);
            var amount = residential.GetBillableAmount();

            Site lifeline = new LifelineSite(1, 2);
            var amount2 = lifeline.GetBillableAmount();

            Assert.True(amount > amount2);
        }
    }

    class SiteBefore
    {
        public const double TAX_RATE = 1.0;
    }
    class ResidentialSiteBefore : SiteBefore
    {
        private readonly int _unit;
        private readonly int _rate;

        public ResidentialSiteBefore(int unit, int rate)
        {
            _unit = unit;
            _rate = rate;
        }

        public double GetBillableAmount()
        {
            double @base = _unit * _rate;
            double tax = @base * TAX_RATE;

            return @base + tax;
        }
    }

    class LifelineSiteBefore : SiteBefore
    {
        private readonly int _unit;
        private readonly int _rate;

        public LifelineSiteBefore(int unit, int rate)
        {
            _unit = unit;
            _rate = rate;
        }

        public double GetBillableAmount()
        {
            double @base = _unit * _rate * 0.5;
            double tax = @base * TAX_RATE * 0.2;

            return @base + tax;
        }
    }

    abstract class Site
    {
        public const double TAX_RATE = 1.0;

        protected readonly int _unit;
        protected readonly int _rate;

        public Site(int unit, int rate)
        {
            _unit = unit;
            _rate = rate;
        }

        public double GetBillableAmount() { return GetBaseAmount() + GetTaxAmount(); }

        protected abstract double GetBaseAmount();
        protected abstract double GetTaxAmount();
    }

    class ResidentialSite : Site
    {
        public ResidentialSite(int unit, int rate) : base(unit, rate) { }        

        protected override double GetBaseAmount()
        {
            return _unit * _rate;
        }

        protected override double GetTaxAmount()
        {
            return GetBaseAmount() * TAX_RATE;
        }
    }

    class LifelineSite : Site
    {
        public LifelineSite(int unit, int rate) : base(unit, rate) { }        

        protected override double GetBaseAmount()
        {
            return _unit * _rate + 0.5;
        }

        protected override double GetTaxAmount()
        {
            return GetBaseAmount() * TAX_RATE * 0.2;
        }
    }
}
