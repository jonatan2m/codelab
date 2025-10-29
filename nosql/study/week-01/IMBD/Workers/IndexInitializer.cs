using IMBD.Database;
using IMBD.Models;
using MongoDB.Driver;

namespace IMBD.Workers
{
    public class IndexInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public IndexInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
            await EnsureIndexes(db, cancellationToken);
            //await CastingMigration.MigrateMovieCastingsAsync(db, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task EnsureIndexes(MongoDbContext db, CancellationToken cancellationToken)
        {
            var reviewsIndexes = await db.Reviews.Indexes.ListAsync();
            var reviewExistingNames = (await reviewsIndexes.ToListAsync()).Select(d => d["name"].AsString).ToHashSet();

            var reviewIdx1 = new CreateIndexModel<Review>(Builders<Review>.IndexKeys.Ascending(r => r.MovieId));
            if (!reviewExistingNames.Contains("movieId_1"))
                await db.Reviews.Indexes.CreateOneAsync(reviewIdx1);

            // composto movieId + userId
            var reviewIdx2 = new CreateIndexModel<Review>(Builders<Review>.IndexKeys.Ascending(r => r.MovieId).Ascending(r => r.Author));
            if (!reviewExistingNames.Contains("movieId_1_author_1"))
                await db.Reviews.Indexes.CreateOneAsync(reviewIdx2);
        }
    }
}