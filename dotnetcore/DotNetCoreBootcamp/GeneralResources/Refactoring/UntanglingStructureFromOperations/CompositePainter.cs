using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.UntanglingStructureFromOperations
{
    /// <summary>
    /// The idea behind this class is to group a list of painters and make them a single object
    /// As the Composite Pattern explains, we need to expose the same interface from the group member, IPainter
    /// We need to Reduce the collection into a single object and in this example, we let the client decide the way
    /// we're going to reduce them.
    /// We could reduce by ourselves, giving the implementation inside of the composite class
    /// </summary>
    /// <typeparam name="TPainter"></typeparam>
    class CompositePainter<TPainter>: IPainter
        where TPainter : IPainter
    {
        public bool IsAvailable => this.Painters.Any(painter => painter.IsAvailable);

        private IEnumerable<TPainter> Painters { get; }

        /// <summary>
        /// This is the way we'll transform a collection into a single object.
        /// Most of the time, we compute the collection values, summing all of them and return a new object
        /// </summary>
        private Func<double, IEnumerable<TPainter>, IPainter> Reduce { get; }

        public CompositePainter(IEnumerable<TPainter> painters,
                                Func<double, IEnumerable<TPainter>, IPainter> reduce)
        {
            this.Painters = painters.ToList();
            this.Reduce = reduce;
        }

        public TimeSpan EstimateTimeToPaint(double sqMeters) =>
            this.Reduce(sqMeters, this.Painters).EstimateTimeToPaint(sqMeters);

        public double EstimateCompensation(double sqMeters) =>
            this.Reduce(sqMeters, this.Painters).EstimateCompensation(sqMeters);
    }
}
