using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace AppDemo.Providers;

public static class RedisOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        var mux = await ConnectionMultiplexer.ConnectAsync(cfg["Redis:ConnectionString"]);
        var db = mux.GetDatabase();
        var session = await db.StringGetAsync("session:cli_1");
        var views = await db.StringGetAsync("pageviews:/home");
        Console.WriteLine($"Redis: session:cli_1={session}");
        Console.WriteLine($"Redis: pageviews:/home={views}");
    }
}
