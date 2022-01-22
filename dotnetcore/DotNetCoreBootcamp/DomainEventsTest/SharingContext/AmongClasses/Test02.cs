using Xunit;
using Xunit.Abstractions;

namespace DomainEventsTest.SharingContext.AmongClasses
{
    [Collection("GameState")]
    public class Test02
    {
        private readonly GameStateFixture _gameStateFixture;
        private readonly ITestOutputHelper _output;

        public Test02(GameStateFixture gameStateFixture, ITestOutputHelper output)
        {
            _gameStateFixture = gameStateFixture;
            _output = output;
        }

        [Fact]
        public void t_03()
        {
            _output.WriteLine($"Game Id {_gameStateFixture.GameState.Id}");

            var p1 = new Player("A");
            var p2 = new Player("B");

            _gameStateFixture.GameState.Players.Add(p1);
            _gameStateFixture.GameState.Players.Add(p2);

            var expectedP1HealthBefore = p1.Health - GameState.EarthquakeDamage;
            var expectedP2HealthBefore = p2.Health - GameState.EarthquakeDamage;

            _gameStateFixture.GameState.Earthquake();

            Assert.Equal(expectedP1HealthBefore, p1.Health);
            Assert.Equal(expectedP2HealthBefore, p2.Health);
        }

        [Fact]
        public void t_04()
        {
            _output.WriteLine($"Game Id {_gameStateFixture.GameState.Id}");

            var p1 = new Player("A");
            var p2 = new Player("B");

            _gameStateFixture.GameState.Players.Add(p1);
            _gameStateFixture.GameState.Players.Add(p2);

            _gameStateFixture.GameState.Reset();

            Assert.Empty(_gameStateFixture.GameState.Players);
        }
    }
}
