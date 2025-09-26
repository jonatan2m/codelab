using Cassandra;
using Microsoft.Extensions.Configuration;

namespace AppSeed.Providers;

public static class CassandraOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        var host = cfg["Cassandra:ContactPoint"]!;
        var port = int.Parse(cfg["Cassandra:Port"] ?? "9042");
        var keyspace = cfg["Cassandra:Keyspace"]!;

        Console.WriteLine("Cassandra: conectando...");
        var cluster = Cluster.Builder().AddContactPoint(host).WithPort(port).Build();
        var session = await cluster.ConnectAsync();

        await session.ExecuteAsync(new SimpleStatement(
            $"CREATE KEYSPACE IF NOT EXISTS {keyspace} WITH replication = {{'class':'SimpleStrategy','replication_factor':1}}"));

        session = await cluster.ConnectAsync(keyspace);

        var table = @"CREATE TABLE IF NOT EXISTS pedidos_por_cliente (
            cliente_id text,
            data timestamp,
            pedido_id text,
            total decimal,
            itens list<text>,
            PRIMARY KEY ((cliente_id), data, pedido_id)
        ) WITH CLUSTERING ORDER BY (data DESC)";
        await session.ExecuteAsync(new SimpleStatement(table));

        var insert = "INSERT INTO pedidos_por_cliente (cliente_id, data, pedido_id, total, itens) VALUES (?,?,?,?,?)";
        var ps = await session.PrepareAsync(insert);
        await session.ExecuteAsync(ps.Bind("cli_1", DateTimeOffset.UtcNow, "ped_cas_1", 3650m, new List<string>{"p1:1x3500","p2:1x150"}));

        Console.WriteLine("Cassandra: seed ok");
    }
}
