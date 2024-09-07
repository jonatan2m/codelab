using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CSharp.Records
{
    public class PlayWithRecords
    {
        [Fact]
        public void Case1()
        {
            SendToOne one = new SendToOne(Guid.NewGuid());
            SendToMore more = new SendToMore(new Guid[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.NotEqual(one.Id, more.Ids[0]);
        }

        [Fact]
        public void Case2()
        {


        }
        private void TesteCase2(SendingAttempt attempt)
        {
            switch (attempt) {
                case SendingAttempt.SentToSome:
                    // Set trackingId and mark as Sent for some recipients
                    // Mark all other recipients as Invalid
                    break;

                case SendingAttempt.SentToNone:
                    // Mark all recipients as Invalid
                    break;

                case SendingAttempt.FailedToSend:
                    // Mark all recipients as Failed
                    break;
            }
        }
    }

    //Case 1
    public record SendToOne(Guid Id);
    public record SendToMore(Guid[] Ids);

    //Case 2 - discriminated unions (ou tagged unions)
    public abstract record SendingAttempt
    {
        private SendingAttempt() { }

        public record SentToSome(Guid TrackingId, IEnumerable<string> Emails): SendingAttempt;
        public record SentToNone() : SendingAttempt;
        public record FailedToSend(string Message) : SendingAttempt;
    }
}
