using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AppDemo.Providers;

public static class MongoOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        var client = new MongoClient(cfg["Mongo:ConnectionString"]);
        var db = client.GetDatabase(cfg["Mongo:Database"]);
        var col = db.GetCollection<BsonDocument>("pedidos");

        var cursor = await col.Aggregate()
            .Group(new BsonDocument { { "_id", "$clienteId" }, { "gasto", new BsonDocument("$sum", "$total") } })
            .ToListAsync();

        Console.WriteLine("Mongo: total por cliente");
        foreach (var d in cursor) Console.WriteLine(d.ToJson());
    }
}
