using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{

    public class PainterBetterWay
    {
        public IPainter FindCheapestPainter(double sqMeters, IEnumerable<IPainter> painters)
        {

            return
                painters
                    .Where(painter => painter.IsAvailable)
                    .Aggregate((IPainter)null, (best, cur) =>
                        best.EstimateCompensation(sqMeters) < cur.EstimateCompensation(sqMeters) ? best : cur);

        }

        public IPainter FindCheapestPainterBetter(double sqMeters, IEnumerable<IPainter> painters)
        {

            return
                painters
                    .Where(painter => painter.IsAvailable)
                    .WithMinimum(painter => painter.EstimateCompensation(sqMeters));
        }

        public IPainter FindCheapestPainterMuchBetter(double sqMeters, IEnumerable<IPainter> painters)
        {

            return
                painters
                    .Where(painter => painter.IsAvailable)
                    .WithMinimumBetter(painter => painter.EstimateCompensation(sqMeters));
        }
    }
}
