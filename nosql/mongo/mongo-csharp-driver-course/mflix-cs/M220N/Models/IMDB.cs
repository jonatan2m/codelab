using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace M220N.Models
{
    public class Imdb
    {
        [BsonElement("id")]
        [JsonProperty("id")]
        public object ImdbId { get; set; }

        public object Votes { get; set; }

        public object Rating { get; set; }
    }
}
