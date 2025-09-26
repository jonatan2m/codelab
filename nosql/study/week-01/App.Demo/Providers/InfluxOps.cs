using InfluxDB.Client;
using Microsoft.Extensions.Configuration;

namespace AppDemo.Providers;

public static class InfluxOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        using var client = InfluxDBClientFactory.Create(cfg["Influx:Url"], cfg["Influx:Token"]!.ToCharArray());
        var query = $"from(bucket: \"{cfg["Influx:Bucket"]}\") |> range(start: -10m) |> filter(fn: (r) => r._measurement == \"cpu\") |> limit(n:5)";
        var tables = await client.GetQueryApi().QueryAsync(query, cfg["Influx:Org"]);

        Console.WriteLine("Influx: últimas leituras cpu");
        foreach (var table in tables)
            foreach (var rec in table.Records)
                Console.WriteLine($"{rec.GetTime()}: {rec.GetValue()}");
    }
}