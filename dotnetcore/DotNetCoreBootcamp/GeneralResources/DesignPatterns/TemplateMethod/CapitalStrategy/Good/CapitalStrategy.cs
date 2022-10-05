using System;
using System.Collections.Generic;
using System.Text;
using DesignPatterns.Strategy.LoanExample;

namespace DesignPatterns.TemplateMethod.CapitalStrategy.Good
{
    public abstract class CapitalStrategy
    {
        public virtual double Capital(Loan loan)
        {
            return loan.GetCommitment() * Duration() * riskFactorFor(loan);
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <returns>always 9</returns>
        public int Duration() => 9;

        protected abstract int riskFactorFor(Loan loan);
    }

    public class CapitalStrategyAdviseLine : CapitalStrategy
    {
        public override double Capital(Loan loan)
        {
            return base.Capital(loan) * loan.GetUnusedPercentage();
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <param name="loan"></param>
        /// <returns>always 3</returns>
        protected override int riskFactorFor(Loan loan)
        {
            return 3;
        }        
    }

    public class CapitalStrategyTermLoan : CapitalStrategy
    {
        /// <summary>
        /// Fake method
        /// </summary>
        /// <param name="loan"></param>
        /// <returns>always 3</returns>
        protected override int riskFactorFor(Loan loan)
        {
            return 3;
        }
    }
}
