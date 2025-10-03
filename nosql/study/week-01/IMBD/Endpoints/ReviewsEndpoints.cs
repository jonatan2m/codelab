using IMBD.Database;
using IMBD.Services;

public static class ReviewsEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/movies/{movieId}/reviews");

        group.MapPost("/seed/{count?}", async (string movieId, int? count, MongoDbContext db, CancellationToken token) =>
        {
            var toCreate = count ?? 10;
            await TestDataService.SeedRandomReviewsAsync(db, movieId, toCreate, token);
            return Results.Ok(new { inserted = toCreate });
        });
    }
}