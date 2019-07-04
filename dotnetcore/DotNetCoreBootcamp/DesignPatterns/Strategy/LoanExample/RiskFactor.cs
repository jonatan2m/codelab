using System;

namespace DesignPatterns.Strategy.LoanExample
{
    internal class RiskFactor
    {
        internal static double ForRiskRating(int rating)
        {
            return rating;
        }
    }
}