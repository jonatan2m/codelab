using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMBD.Database;
using IMBD.Models;
using IMBD.Models.Requests;
using IMBD.Services;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IMBD.Endpoints
{
    public static class MoviesEndpoints
    {
        public static void MapMovieEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/movies");
            
            group.MapGet("/", async (MongoDbContext db) =>
            {
                var list = await db.Movies.Find(_ => true).ToListAsync();
                return Results.Ok(list);
            });
            group.RequireRateLimiting("reviews-policy");

            group.MapGet("/{id}", async (string id, MongoDbContext db) =>
            {
                var stats = await GetSummary(db, id);
                var movie = await db.Movies.Find(m => m.Id == id).FirstOrDefaultAsync();
                return movie is not null ? Results.Ok(new
                {
                    reviews = new
                    {
                        stats.total,
                        stats.media
                    },
                    movie
                }) : Results.NotFound();
            });

            group.MapGet("/{id}/reviews", async (MongoDbContext db, string id, int skip = 0, int limit = 50) =>
            {
                var reviews = await db.Reviews
                    .Find(m => m.MovieId == id)
                    .Skip(skip)
                    .Limit(limit)
                    .ToListAsync();
                return reviews is not null ? Results.Ok(reviews) : Results.NotFound();
            });

            group.MapGet("/{id}/stats", async (MovieStatsService movieStatsService, MongoDbContext db, string id) =>
            {
                var movieStats = await db.MovieStats.Find(x => x.MovieId == id).FirstOrDefaultAsync();

                if(movieStats is null)
                {
                    //faria sentido que esse processo fosse feito em background
                    //precisa entender como recalcular os valores, 
                    //mas aí talvez tenha que seja por algoritmo e não pelo banco
                    await movieStatsService.RecalculateStatsAsync(id);                    
                }
                    
                return movieStats is not null ? Results.Ok(movieStats) : Results.NotFound();
            });

            group.MapPost("/", async (CreateMovieRequest req,
             MongoDbContext db, CancellationToken token) =>
            {
                if (string.IsNullOrWhiteSpace(req.Title))
                    return Results.BadRequest(new { error = "Title is required." });

                var movie = new Movie
                {
                    Title = req.Title,
                    Year = req.Year,
                    ReleaseDate = req.ReleaseDate,
                    Castings = req.Castings ?? new List<string>()
                };

                await db.Movies.InsertOneAsync(movie, cancellationToken: token);
                return Results.Created($"/movies/{movie.Id}", movie);
            });
        }

        public static async Task<(long total, double media)> GetSummary(MongoDbContext db, string movieId)
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("movieId", new ObjectId(movieId))),
                new BsonDocument("$group", new BsonDocument{
                    {"_id", "$movieId"},
                    { "total", new BsonDocument("$sum", 1) },
                    { "media", new BsonDocument("$avg", "$rating") }
                })
            };

            var result = await db.Reviews.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

            if (result is null) return (0, 0.0);

            return (
                total: result["total"].AsInt32,
                media: result["media"].ToDouble()
            );
        }

        public static async Task<Dictionary<int, int>> GetDistribuicaoPorNotaAsync(MongoDbContext db, string movieId)
        {
            var pipeline = new[]
                   {
            new BsonDocument("$match", new BsonDocument("movieId", new ObjectId(movieId))),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$rating" },
                { "quantidade", new BsonDocument("$sum", 1) }
            }),
            new BsonDocument("$sort", new BsonDocument("_id", -1))
        };

            var results = await db.Reviews.Aggregate<BsonDocument>(pipeline).ToListAsync();

            return results.ToDictionary(
                x => x["_id"].ToInt32(),
                x => x["quantidade"].ToInt32()
            );
        }
    }
}