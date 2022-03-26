using System.Collections.Generic;
using Xunit;

namespace Refactoring.UntanglingStructureFromOperations
{
    public class Test
    {
        [Fact]
        public void Just_instantiate_the_painter_group()
        {
            IEnumerable<ProportionalPainter> painters = new ProportionalPainter[10];

            IPainter painter = CompositePainterFactories.CreateGroup(painters);
        }
    }
}
