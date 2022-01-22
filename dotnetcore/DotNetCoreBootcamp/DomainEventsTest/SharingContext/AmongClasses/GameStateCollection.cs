using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainEventsTest.SharingContext.AmongClasses
{
    [CollectionDefinition("GameState")]
    public class GameStateCollection: ICollectionFixture<GameStateFixture>
    {
    }
}
