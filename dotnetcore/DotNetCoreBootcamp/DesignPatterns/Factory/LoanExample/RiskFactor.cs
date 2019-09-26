using System;

namespace DesignPatterns.Factory.LoanExample
{
    internal class RiskFactor
    {
        internal static double ForRiskRating(int rating)
        {
            return rating;
        }
    }
}