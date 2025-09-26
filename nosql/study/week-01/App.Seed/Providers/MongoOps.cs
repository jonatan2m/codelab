using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AppSeed.Providers;

public static class MongoOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        var conn = cfg["Mongo:ConnectionString"]!;
        var dbName = cfg["Mongo:Database"]!;
        var client = new MongoClient(conn);
        var db = client.GetDatabase(dbName);

        Console.WriteLine("Mongo: conectando...");
        var clientes = db.GetCollection<BsonDocument>("clientes");
        var pedidos  = db.GetCollection<BsonDocument>("pedidos");

        await clientes.InsertManyAsync(new[]
        {
            new BsonDocument { { "_id", "cli_1" }, { "nome", "Ana" }, { "email", "ana@ex.com" } },
            new BsonDocument { { "_id", "cli_2" }, { "nome", "Bruno" }, { "email", "bruno@ex.com" } }
        });

        await pedidos.InsertManyAsync(new[]
        {
            new BsonDocument {
                { "_id", "ped_1" }, { "clienteId", "cli_1" }, { "total", 3650 },
                { "itens", new BsonArray {
                    new BsonDocument{{"produtoId","p1"},{"nome","Notebook"},{"qtd",1},{"preco",3500}},
                    new BsonDocument{{"produtoId","p2"},{"nome","Mouse"},{"qtd",1},{"preco",150}}
                }},
                { "data", DateTime.UtcNow }
            }
        });

        await pedidos.Indexes.CreateOneAsync(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("clienteId").Descending("data")));

        Console.WriteLine("Mongo: seed ok");
    }
}
