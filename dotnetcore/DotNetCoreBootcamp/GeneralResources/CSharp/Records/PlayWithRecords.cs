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
    }

    //Case 1
    public record SendToOne(Guid Id);
    public record SendToMore(Guid[] Ids);
}
