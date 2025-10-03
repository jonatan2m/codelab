using IMBD.Database;
using IMBD.Endpoints;
using IMBD.Services;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapMovieEndpoints();
app.MapReviewEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
