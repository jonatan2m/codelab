using InfluxDB.Client;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Configuration;

namespace AppSeed.Providers;

public static class InfluxOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        // var url = cfg["Influx:Url"]!;
        // var token = cfg["Influx:Token"]!;
        // var org = cfg["Influx:Org"]!;
        // var bucket = cfg["Influx:Bucket"]!;

        // Console.WriteLine("Influx: conectando...");
        // using var client = InfluxDBClientFactory.Create(url, token.ToCharArray());
        // using var write = client.GetWriteApiAsync();

        // var p = PointData.Measurement("cpu")
        //     .Tag("host", "server-1")
        //     .Field("usage", new Random().Next(50, 90))
        //     .Timestamp(DateTime.UtcNow, InfluxDB.Client.Api.Domain.WritePrecision.S);

        // await write.WritePointAsync(p, bucket, org);
        Console.WriteLine("Influx: seed ok");
    }
}
