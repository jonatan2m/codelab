using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.PatternMatching
{
    /// <summary>
    /// https://docs.microsoft.com/pt-br/dotnet/csharp/whats-new/csharp-8
    /// </summary>
    public class EliminateIfs
    {
        public static decimal PeakTime(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.MorningRush, true) => 2.00m,
                (true, TimeBand.EveningRush, false) => 2.00m,
                (true, TimeBand.MorningRush, false) => 1.00m,
                (true, TimeBand.EveningRush, true) => 1.00m,
                (true, TimeBand.Daytime, _) => 1.50m,
                (true, TimeBand.Overnight, _) => 0.75m,
                (false, _, _) => 1.00m
            }; 

        public static decimal PeakTimeImperative(DateTime timeOfToll, bool inbound)
        {
            if (IsWeekDay(timeOfToll))
            {
                var timeBand = GetTimeBand(timeOfToll);
                if (inbound)
                {
                    if (timeBand == TimeBand.MorningRush)
                        return 2.00m;
                    if (timeBand == TimeBand.Daytime)
                        return 1.50m;
                    if (timeBand == TimeBand.EveningRush)
                        return 1.00m;
                    return 0.75m;
                }
                else
                {
                    if (timeBand == TimeBand.MorningRush)
                        return 1.00m;
                    if (timeBand == TimeBand.Daytime)
                        return 1.50m;
                    if (timeBand == TimeBand.EveningRush)
                        return 2.00m;
                    return 0.75m;
                }
            }
            else
            {
                return 1.00m;
            }
        }

        private static bool IsWeekDay(DateTime time) =>
            time.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };

        private static TimeBand GetTimeBand(DateTime timeOfToll)
        {
            int hour = timeOfToll.Hour;
            if (hour < 6) return TimeBand.Overnight;
            if (hour < 10) return TimeBand.MorningRush;
            if (hour < 16) return TimeBand.Daytime;
            if (hour < 20) return TimeBand.EveningRush;

            return TimeBand.Overnight;
        }

        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

    }
}
