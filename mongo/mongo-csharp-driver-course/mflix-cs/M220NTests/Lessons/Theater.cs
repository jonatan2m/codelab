using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M220NLessons
{
    /* Note: this is the mapping class for the Theater object stored in the
     * "theaters" collection. We don't use the Theater object elsewhere
     * in this course; if we did, this mapping class would live with the
     * others in the Models folder of the M220N project, rather that in this
     * Test class.
     */
    class Theater
    {
        public ObjectId Id { get; set; }
        public int TheaterId { get; set; }
        public LocationType Location { get; set; }

        public Theater() { }

        public Theater(int theaterId, string street, string city, string state, string zip)
        {
            this.TheaterId = theaterId;
            this.Location = new LocationType(street, city, state, zip);
        }

        public class LocationType
        {
            public AddressType Address { get; set; }
            public GeoLoc Geo { get; set; }

            public LocationType(string street, string city, string state, string zip)
            {
                this.Address = new LocationType.AddressType()
                {
                    Street1 = street,
                    City = city,
                    State = state,
                    Zipcode = zip
                };
            }

            public class AddressType
            {
                public string Street1 { get; set; }
                public string City { get; set; }
                public string State { get; set; }
                public string Zipcode { get; set; }
            }

            public class GeoLoc
            {
                public string Type { get; set; }
                public double[] Coordinates { get; set; }
            }
        }
    }
}
