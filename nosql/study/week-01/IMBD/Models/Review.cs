using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IMBD.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //Vou inserir essa relação no futuro
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MovieId { get; set; }

        public string Author { get; set; }
        public string Content { get; set; }
        public double? Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}