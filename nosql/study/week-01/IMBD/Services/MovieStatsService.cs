using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMBD.Database;
using IMBD.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IMBD.Services
{
    public class MovieStatsService
    {
        private readonly IMongoCollection<Review> _reviews;
        private readonly IMongoCollection<MovieStats> _stats;

        public MovieStatsService(MongoDbContext context)
        {
            _reviews = context.Reviews;
            _stats = context.MovieStats;
        }

        public async Task RecalculateStatsAsync(string movieId)
        {
            var objectId = new ObjectId(movieId);

            // Etapa 1: total e média
            var summaryPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("movieId", objectId)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$movieId" },
                    { "total", new BsonDocument("$sum", 1) },
                    { "media", new BsonDocument("$avg", "$rating") }
                })
            };

            var summary = await _reviews.Aggregate<BsonDocument>(summaryPipeline).FirstOrDefaultAsync();
            if (summary is null) return;

            // Etapa 2: distribuição
            var histogramaPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("movieId", objectId)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$rating" },
                    { "quantidade", new BsonDocument("$sum", 1) }
                })
            };

            var histograma = await _reviews.Aggregate<BsonDocument>(histogramaPipeline).ToListAsync();

            var distrib = new Dictionary<string, int>();

            foreach (var doc in histograma)
            {
                int nota = doc["_id"].IsDouble
                    ? Convert.ToInt32(doc["_id"].AsDouble)
                    : doc["_id"].ToInt32();

                if (!distrib.TryAdd(nota.ToString(), doc["quantidade"].ToInt32()))
                    distrib[nota.ToString()] += doc["quantidade"].ToInt32();
            }

            var stats = new MovieStats
            {
                MovieId = movieId,
                AverageRating = Math.Round(summary["media"].ToDouble(), 2),
                TotalReviews = summary["total"].ToInt64(),
                DistributionRatings = distrib,
                LastUpdated = DateTime.UtcNow
            };

            // upsert
            await _stats.ReplaceOneAsync(
                x => x.MovieId == movieId,
                stats,
                new ReplaceOptions { IsUpsert = true }
            );
        }
    }
}