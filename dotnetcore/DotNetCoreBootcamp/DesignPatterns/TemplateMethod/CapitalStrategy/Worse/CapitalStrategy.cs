using System;
using System.Collections.Generic;
using System.Text;
using DesignPatterns.Strategy.LoanExample;

namespace DesignPatterns.TemplateMethod.CapitalStrategy.Worse
{
    public abstract class CapitalStrategy
    {
        public abstract double Capital(Strategy.LoanExample.Loan loan);

        /// <summary>
        /// Fake method
        /// </summary>
        /// <returns>always 9</returns>
        public int Duration() => 9;
    }

    public class CapitalStrategyAdviseLine : CapitalStrategy
    {
        public override double Capital(Loan loan)
        {
            return loan.GetCommitment() * loan.GetUnusedPercentage() * Duration() * riskFactorFor(loan);
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <param name="loan"></param>
        /// <returns>always 3</returns>
        private int riskFactorFor(Loan loan)
        {
            return 3;
        }        
    }

    public class CapitalStrategyTermLoan : CapitalStrategy
    {
        public override double Capital(Loan loan)
        {
            return loan.GetCommitment() * Duration() * riskFactorFor(loan);
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <param name="loan"></param>
        /// <returns>always 3</returns>
        private int riskFactorFor(Loan loan)
        {
            return 3;
        }
    }
}
