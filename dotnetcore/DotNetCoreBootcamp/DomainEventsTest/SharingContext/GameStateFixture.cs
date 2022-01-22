using System;
using Xunit;

namespace DomainEventsTest.SharingContext
{
    public class GameStateFixture : IDisposable
    {
        public readonly GameState GameState;

        public GameStateFixture()
        {
            GameState = new GameState();
        }

        public void Dispose()
        {
            //Cleanup
        }
    }
}