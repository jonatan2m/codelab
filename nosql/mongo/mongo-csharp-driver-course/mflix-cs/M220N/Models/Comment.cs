using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace M220N.Models
{
    public class Comment
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        public ObjectId Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        [BsonElement("movie_id")]
        [JsonProperty("movie_id")]
        [JsonIgnore]
        public ObjectId MovieId { get; set; }
    }
}
