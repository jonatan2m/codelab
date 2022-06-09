using System.Collections.Generic;
using System.Dynamic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M220N.Models.Projections
{
    public class MovieByCountryProjection
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }
    }

    public class MovieByTextProjection : Movie
    {
        [BsonElement("score")]
        public double Score { get; set; }
    }

    public class MoviesByCastProjection
    {
        public List<Movie> Movies { get; set; }

        public List<ExpandoObject> Rating { get; set; }

        public List<ExpandoObject> Runtime { get; set; }

        public int Count { get; set; }
    }
}
