using IMBD.Database;
using IMBD.Endpoints;
using IMBD.Services;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var pack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new IgnoreIfNullConvention(true),
    new IgnoreExtraElementsConvention(true),
    new EnumRepresentationConvention(BsonType.String)
};

ConventionRegistry.Register("AppConventions", pack, t => true);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoDbSettings:ConnectionString"];

    var client = new MongoClient(connectionString);
    return client;
});

builder.Services.AddScoped(sp =>
{
    var databaseName = builder.Configuration["MongoDbSettings:DatabaseName"];
    return sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName);
});

builder.Services.AddScoped(sp =>
new MongoDbContext(sp.GetRequiredService<IMongoDatabase>()));

builder.Services.AddScoped<MovieStatsService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRateLimiter(opts =>
{
    opts.AddFixedWindowLimiter("reviews-policy", l => { l.PermitLimit = 1; l.Window = TimeSpan.FromSeconds(5); });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue("X-Api-Key", out var key) || key != "minha-chave")
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }
    await next();
});

app.UseRateLimiter();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapMovieEndpoints();
app.MapReviewEndpoints();

app.Run();

