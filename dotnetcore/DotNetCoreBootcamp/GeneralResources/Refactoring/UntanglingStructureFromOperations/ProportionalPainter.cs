using System;

namespace Refactoring.UntanglingStructureFromOperations
{
    class ProportionalPainter : IPainter
    {
        public TimeSpan TimePerSqMeter { get; set; }
        public double DollarsPerHour { get; set; }

        public bool IsAvailable => true;

        public TimeSpan EstimateTimeToPaint(double sqMeters) =>
            TimeSpan.FromHours(this.TimePerSqMeter.TotalHours*sqMeters);

        public double EstimateCompensation(double sqMeters) =>
            this.EstimateTimeToPaint(sqMeters).TotalHours * this.DollarsPerHour;
    }
}