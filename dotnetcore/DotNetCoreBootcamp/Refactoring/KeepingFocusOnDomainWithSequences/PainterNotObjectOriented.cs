using System.Collections.Generic;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{
    public class PainterNotObjectOriented
    {
        public IPainter FindCheapestPainter(double sqMeters, IEnumerable<IPainter> painters)
        {
            double bestPrice = 0;
            IPainter cheapest = null;

            foreach (var painter in painters)
            {
                if (painter.IsAvailable)
                {
                    double price = painter.EstimateCompensation(sqMeters);
                    if (cheapest == null || price < bestPrice)
                    {
                        cheapest = painter;
                        bestPrice = price;
                    }
                }
            }

            return cheapest;
        }
    }
}