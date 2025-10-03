using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IMBD.Models
{
    public class Casting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int Age => DateTime.Today.Year - DateOfBirth.Year;        
    }
}