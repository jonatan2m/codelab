using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMBD.Models;
using MongoDB.Driver;

namespace IMBD.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IMongoDatabase db) => _db = db;        

        public IMongoCollection<Movie> Movies => _db.GetCollection<Movie>("movies");
        public IMongoCollection<MovieStats> MovieStats => _db.GetCollection<MovieStats>("movie_stats");
        public IMongoCollection<Casting> Castings => _db.GetCollection<Casting>("castings");
        public IMongoCollection<Review> Reviews => _db.GetCollection<Review>("reviews");
        public IMongoCollection<User> Users => _db.GetCollection<User>("users");
    }
}