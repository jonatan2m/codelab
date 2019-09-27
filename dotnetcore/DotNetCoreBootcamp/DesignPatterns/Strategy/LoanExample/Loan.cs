using System;

namespace DesignPatterns.Strategy.LoanExample
{
    public class TermCapital : CapitalStrategy
    {
        private readonly double unusedPercentage = 1.0;
        public override double riskAmount()
        {
            return loan.outstanding + calcUnusedRiskAmount(unusedPercentage);
        }

        public override double duration()
        {
            return calcDuration(loan.maturity, loan.start);
        }
    }

    public class RevolverCapital : CapitalStrategy
    {
        public override double riskAmount()
        {
            var unusedPercentage = loan.rating > 4 ? 0.75 : 0.25;
            return loan.outstanding + calcUnusedRiskAmount(unusedPercentage);
        }

        public override double duration()
        {
            return calcDuration(loan.expiry, loan.start);
        }
    }

    public class RCTLCapital : CapitalStrategy
    {
        public override double riskAmount()
        {
            var unusedPercentage = loan.rating > 4 ? 0.95 : 0.50;
            return loan.outstanding + calcUnusedRiskAmount(unusedPercentage);            
        }
    }

    public abstract class CapitalStrategy
    {
        protected Loan loan;
        protected static int MILLIS_PER_DAY = 86400000;
        protected static int DAYS_PER_YEAR = 365;

        public abstract double riskAmount();

        public double calc(Loan loan)
        {
            this.loan = loan;
            return riskAmount() * duration() * RiskFactor.ForRiskRating(loan.rating);
        }

        protected double calcUnusedRiskAmount(double unusedPercentage)
        {
            return (loan.notional - loan.outstanding) * unusedPercentage;
        }

        public virtual double duration()
        {            
            double revolverDuration = calcDuration(loan.expiry, loan.start);
            double termDuration = calcDuration(loan.maturity, loan.expiry);
            return revolverDuration + termDuration;

        }

        protected double calcDuration(DateTime? from, DateTime? to)
        {
            return (TotalMilliseconds(from, to) / MILLIS_PER_DAY) / DAYS_PER_YEAR;
        }

        private double TotalMilliseconds(DateTime? start, DateTime? end)
        {
            return (start.Value - end.Value).TotalMilliseconds;
        }
    }

    public class Loan
    {
        public double notional { get; set; }
        public double outstanding { get; set; }
        public int rating { get; set; }
        public DateTime? start { get; set; }
        public DateTime? expiry { get; set; }
        public DateTime? maturity { get; set; }
        private CapitalStrategy capitalStrategy;

        public Loan(double notional, DateTime? start, DateTime? expiry,
            DateTime? maturity, int rating, CapitalStrategy capitalStrategy)
        {
            this.notional = notional;
            this.start = start;
            this.expiry = expiry;
            this.maturity = maturity;
            this.rating = rating;
            this.capitalStrategy = capitalStrategy;
        }

        public double calcCapital()
        {
            return capitalStrategy.calc(this);
        }

        public void setOutstanding(double newOutstanding)
        {
            outstanding = newOutstanding;
        }

        public static Loan newTermLoan(double notional, DateTime? start, DateTime? maturity, int rating)
        {            
            return new Loan(notional, start, null, maturity, rating, new TermCapital());
        }

        public static Loan newRCTL(double notional, DateTime? start, DateTime? expiry,
            DateTime? maturity, int rating)
        {
            return new Loan(notional, start, expiry, maturity, rating, new RCTLCapital());
        }
        public static Loan newRevolver(double notional, DateTime? start, DateTime? expiry, int rating)
        {            
            return new Loan(notional, start, expiry, null, rating, new RevolverCapital());
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <returns>always 5</returns>
        internal int GetCommitment()
        {
            return 5;
        }

        /// <summary>
        /// Fake method
        /// </summary>
        /// <returns>always 7</returns>
        internal int GetUnusedPercentage()
        {
            return 7;
        }
    }
}