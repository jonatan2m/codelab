using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace M220N.Models
{
    public class Critic
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("count")]
        [BsonElement("count")]
        public int NumComments { get; set; }
    }
}
