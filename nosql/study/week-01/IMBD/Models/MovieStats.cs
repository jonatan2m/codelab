using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IMBD.Models
{
    /// <summary>
    /// Será preenchido por um Job agendado, de modo que as collections se mantenham atualizadas
    /// </summary>
    public class MovieStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MovieId { get; set; }

        public long TotalReviews { get; set; }

        public double AverageRating { get; set; }

        public Dictionary<string, int> DistributionRatings { get; set; }

        public DateTime LastUpdated { get; set; }
    }

}