//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DesignPatterns.Strategy.LoanExample
//{
//    public class TermLoanCapital : CapitalStrategy
//    {
//        public override double Duration()
//        {
//            return CalcDuration(_loan.maturity.Value, _loan.start.Value);
//        }

//        public override double RiskAmount()
//        {
//            return _loan.Outstanding;
//        }
//    }

//    public class RevolverCapital : CapitalStrategy
//    {
//        public override double Duration()
//        {
//            return CalcDuration(_loan.expiry.Value, _loan.start.Value);
//        }

//        public override double RiskAmount()
//        {
//            return _loan.Outstanding + CalcUnusedRiskAmount();
//        }

//        private double CalcUnusedRiskAmount()
//        {
//            return (_loan.Notional - _loan.Outstanding) * CalcUnusedPercentage();
//        }

//        private double CalcUnusedPercentage()
//        {
//            return _loan.rating > 4 ? 0.75 : 0.25;
//        }
//    }

//    public class RCTLCapital : CapitalStrategy
//    {
//        public override double Duration()
//        {                       
//            double revolverDuration = CalcDuration(_loan.expiry.Value, _loan.start.Value);
//            double termDuration = CalcDuration(_loan.maturity.Value, _loan.expiry.Value);
//            return revolverDuration + termDuration;
//        }

//        public override double RiskAmount()
//        {
//            return _loan.Outstanding + CalcUnusedRiskAmount();
//        }

//        private double CalcUnusedRiskAmount()
//        {
//            return (_loan.Notional - _loan.Outstanding) * CalcUnusedPercentage();
//        }

//        private double CalcUnusedPercentage()
//        {
//            return  _loan.rating > 4 ? 0.95 : 0.50;

//        }
//    }

//    public abstract class CapitalStrategy
//    {
//        protected Loan _loan;
//        protected readonly int MILLIS_PER_DAY = 86400000;
//        protected readonly int DAYS_PER_YEAR = 365;

//        public abstract double Duration();
//        public abstract double RiskAmount();

//        protected double CalcDuration(DateTime start, DateTime end)
//        {
//            return (TotalMilliseconds(start, end) / MILLIS_PER_DAY) / DAYS_PER_YEAR;
//        }

//        public double Calc(Loan loan)
//        {
//            _loan = loan;
//            return RiskAmount() * Duration() * RiskFactor.ForRiskRating(loan.rating);
//        }

//        private double TotalMilliseconds(DateTime? start, DateTime? end)
//        {
//            return (start.Value - end.Value).TotalMilliseconds;
//        }
//    }
//    public class LoanFromBook
//    {
//        public double Notional { get; private set; }
//        public double Outstanding { get; private set; }
//        public int rating;
//        public readonly DateTime? start;
//        public readonly DateTime? expiry;
//        public readonly DateTime? maturity;
//        private CapitalStrategy capitalStrategy;


//        protected Loan(
//            double notional, DateTime? start, DateTime? expiry,
//            DateTime? maturity, int rating, CapitalStrategy capitalStrategy)
//        {
//            Notional = notional;
//            this.start = start;
//            this.expiry = expiry;
//            this.maturity = maturity;
//            this.rating = rating;
//            this.capitalStrategy = capitalStrategy;
//        }

//        public double CalcCapital()
//        {
//            return capitalStrategy.Calc(this);
//        }

//        public void SetOutstanding(double newOutstanding)
//        {
//            Outstanding = newOutstanding;
//        }

//        public static Loan newTermLoan(double notional, DateTime? start, DateTime? maturity, int rating)
//        {
//            return new Loan(notional, start, null, maturity, rating, new TermLoanCapital());
//        }

//        public static Loan newRCTL(double notional, DateTime? start,
//            DateTime? expiry, DateTime? maturity, int rating)
//        {
//            return new Loan(notional, start, expiry, maturity, rating, new RCTLCapital());
//        }
//        public static Loan newRevolver(double notional, DateTime? start, DateTime? expiry,
//        int rating)
//        {
//            return new Loan(notional, start, expiry, null, rating, new RevolverCapital());
//        }

//    }
//}
