using System.Collections.Generic;
using System.Linq;

namespace Refactoring.UntanglingStructureFromOperations
{
    class Painters
    {

        private IEnumerable<IPainter> ContainedPainters { get; }

        public Painters(IEnumerable<IPainter> painters)
        {
            this.ContainedPainters = painters.ToList();
        }

        public Painters GetAvailable() =>
            //creating another instance of Painters, we keep the sequence safer
            new Painters(this.ContainedPainters.Where(painter => painter.IsAvailable));

        public Painters GetAvailableUsingLessMemory()
        {
            //Regarding the length of sequence, It could make sense, even if the execution time increase a little.
            if (ContainedPainters.All(painter => painter.IsAvailable))
                return this;

            //creating another instance of Painters, we keep the sequence safer
            return new Painters(this.ContainedPainters.Where(painter => painter.IsAvailable));
        }

        //Group what client wants to know/do with IPainter.
        //In this case, the client wants to get the cheapest painter and/or the fastest painter.

        public IPainter GetCheapestOne(double sqMeters) =>
            this.ContainedPainters.WithMinimum(painter => painter.EstimateCompensation(sqMeters));

        public IPainter GetFastestOne(double sqMeters) =>
            this.ContainedPainters.WithMinimum(painter => painter.EstimateTimeToPaint(sqMeters));
    }
}