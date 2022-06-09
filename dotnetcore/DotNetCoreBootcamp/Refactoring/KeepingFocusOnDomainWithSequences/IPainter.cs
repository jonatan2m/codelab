using System;
using System.Text;
using Moq;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{
    public interface IPainter
    {
        bool IsAvailable { get; set; }
        TimeSpan EstimateTimeToPaint(double sqMeters);
        double EstimateCompensation(double sqMeters);
    }

    public class Painter : IPainter
    {
        private Random random;
        public bool IsAvailable { get; set; }

        public Painter()
        {
            random = new Random();
        }

        public TimeSpan EstimateTimeToPaint(double sqMeters)
        {
            return TimeSpan.FromDays(Math.Ceiling(sqMeters * random.NextDouble()));
        }

        public double EstimateCompensation(double sqMeters)
        {
            return sqMeters * random.Next(3);
        }
    }
}