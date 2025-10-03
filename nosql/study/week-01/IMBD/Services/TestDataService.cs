using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using IMBD.Database;
using IMBD.Models;

namespace IMBD.Services
{
    public static class TestDataService
    {
        /// <summary>
        /// Gera e insere 'count' reviews aleatórios para o movieId informado.
        /// </summary>
        public static async Task SeedRandomReviewsAsync(MongoDbContext db, string movieId, int count = 10, CancellationToken token = default)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (string.IsNullOrWhiteSpace(movieId)) throw new ArgumentException("movieId is required", nameof(movieId));
            if (count <= 0) return;

            var faker = new Faker<Review>()
                .RuleFor(r => r.Id, f => null) // driver irá gerar o _id
                .RuleFor(r => r.MovieId, f => movieId)
                .RuleFor(r => r.Author, f => f.Person.FullName)
                .RuleFor(r => r.Content, f => f.Lorem.Paragraphs(f.Random.Int(1, 3)))
                .RuleFor(r => r.Rating, f => Math.Round(f.Random.Double(1, 10), 1))
                .RuleFor(r => r.CreatedAt, f => f.Date.Past(5, DateTime.UtcNow));

            var reviews = faker.Generate(count);
            await db.Reviews.InsertManyAsync(reviews, cancellationToken: token);
        }
    }
}