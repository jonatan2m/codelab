using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.SnapshotTesting
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid InternalCode { get; set; }
    }
    public class ClassToBeTested
    {
        public static Person GetPerson()
        {
            return new()
            {
                Id = 2,
                Name = "Test",
                Description = "Test",
                Age = 41,
                BirthDate = new DateTime(2022, 12, 22),
                InternalCode = Guid.NewGuid()
            };
        }
    }

    [UsesVerify]
    public class SnapshotTest
    {
        [Fact]
        public async Task GetPersonTest()
        {
            //https://github.com/VerifyTests/Verify/blob/main/docs/serializer-settings.md
            var settings = new VerifySettings();
            settings.DontScrubDateTimes();            

            var person = ClassToBeTested.GetPerson();

            await Verify(person, settings);
        }
    }
}
