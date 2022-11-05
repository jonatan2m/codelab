using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.RaiseEventOrThrowException
{
    public class RaiseTests
    {
        [Fact]
        public async Task RaiseEventAssertions()
        {
            var messageSender = new Message();

            var receivedEvent = Assert.Raises<MessageEventArgs>(
                a => messageSender.SendMessageEvent += a,
                b => messageSender.SendMessageEvent -= b,
                () => messageSender.SendMessageToUser("This is an event message"));
            Assert.NotNull(receivedEvent);
            Assert.Equal("This is an event message", receivedEvent.Arguments.Message);

            var receivedEvent2 = Assert.RaisesAny<MessageEventArgs>(
               a => messageSender.SendMessageEvent += a,
               b => messageSender.SendMessageEvent -= b,
               () => messageSender.SendMessageToUser("This is an event message"));
            Assert.NotNull(receivedEvent2);
            Assert.Equal("This is an event message", receivedEvent2.Arguments.Message);

            var receivedEventTask = Assert.RaisesAsync<MessageEventArgs>(
              a => messageSender.SendMessageEvent += a,
              b => messageSender.SendMessageEvent -= b,
              async () => messageSender.SendMessageToUser("This is an event message"));

            var receivedEventAsync = await receivedEventTask;
            Assert.NotNull(receivedEventAsync);
            Assert.Equal("This is an event message", receivedEventAsync.Arguments.Message);
        }
    }    
}
