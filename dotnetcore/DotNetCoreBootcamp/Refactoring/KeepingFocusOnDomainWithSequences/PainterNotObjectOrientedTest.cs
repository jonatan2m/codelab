using System.Linq;
using AutoFixture;
using Xunit;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{
    public class PainterNotObjectOrientedTest
    {
        private PainterNotObjectOriented _painterManagement;
        private Fixture _fixture;

        public PainterNotObjectOrientedTest()
        {
            _painterManagement = new PainterNotObjectOriented();
            _fixture = new Fixture();
        }

        [Fact]
        public void Given_empty_list_of_painters_should_return_null()
        {
            // Arrange
            var painters = Enumerable.Empty<IPainter>();

            // Act
            var cheapest = _painterManagement.FindCheapestPainter(sqMeters: 12, painters);

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
            var cheapest = _painterManagement.FindCheapestPainter(sqMeters: 12, painters);

            // Assert
            Assert.NotNull(cheapest);
            Assert.Equal(painters[0], cheapest);
        }
    }
}