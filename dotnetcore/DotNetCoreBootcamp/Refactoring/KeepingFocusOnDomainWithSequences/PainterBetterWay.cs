using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Moq;
using Xunit;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{
    public static class EnumerableExtension
    {
        public static T WithMinimum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion)
        where T : class
        where TKey : IComparable<TKey>
        {
            return sequence
                .Aggregate((T)null, (best, cur) =>
                    best == null || criterion(cur).CompareTo(criterion(best)) < 0 ? cur : best);

        }

        public static T WithMinimumBetter<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion)
            where T : class
            where TKey : IComparable<TKey>
        {
            var result = sequence
                .Select(x => Tuple.Create(x, criterion(x)))
                .Aggregate((Tuple<T, TKey>) null,
                    (best, cur) => best == null || cur.Item2.CompareTo(best.Item2) < 0 ? cur : best);

            return result == null ? default(T) : result.Item1;
        }
    }

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

    public class PainterBetterWayTest
    {
        private PainterBetterWay _painterManagement;
        private Fixture _fixture;

        public PainterBetterWayTest()
        {
            _painterManagement = new PainterBetterWay();
            _fixture = new Fixture();
        }

        [Fact]
        public void Given_empty_list_of_painters_should_return_null()
        {
            // Arrange
            var painters = Enumerable.Empty<IPainter>();

            // Act
            var cheapest = _painterManagement.FindCheapestPainterMuchBetter(sqMeters: 12, painters);

            // Arrange
            Assert.Null(cheapest);
        }

        [Fact]
        public void Given_a_list_of_painters_should_return_the_cheapest()
        {
            // Arrange
            var painters = _fixture.Build<Painter>()
                .With(x => x.IsAvailable, false)
                .CreateMany(3)
                .ToList();

            painters[0].IsAvailable = true;

            // Act
            var cheapest = _painterManagement.FindCheapestPainterMuchBetter(sqMeters: 12, painters);

            // Assert
            Assert.NotNull(cheapest);
            Assert.Equal(painters[0], cheapest);
        }
    }
}
