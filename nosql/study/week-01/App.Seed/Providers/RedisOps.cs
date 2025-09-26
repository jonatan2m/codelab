using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace AppSeed.Providers;

public static class RedisOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        var cs = cfg["Redis:ConnectionString"]!;
        Console.WriteLine("Redis: conectando...");
        var mux = await ConnectionMultiplexer.ConnectAsync(cs);
        var db = mux.GetDatabase();

        await db.StringSetAsync("session:cli_1", "{\"token\":\"x-123\"}");
        await db.StringIncrementAsync("pageviews:/home");
        Console.WriteLine("Redis: seed ok");
    }
}
