using Cassandra;
using Microsoft.Extensions.Configuration;

namespace AppDemo.Providers;

public static class CassandraOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint(cfg["Cassandra:ContactPoint"])
            .WithPort(int.Parse(cfg["Cassandra:Port"] ?? "9042"))
            .Build();

        var session = await cluster.ConnectAsync(cfg["Cassandra:Keyspace"]);
        var rs = await session.ExecuteAsync(new SimpleStatement(
            "SELECT cliente_id, data, pedido_id, total FROM pedidos_por_cliente WHERE cliente_id='cli_1' LIMIT 5"));

        Console.WriteLine("Cassandra: últimos pedidos cli_1");
        foreach (var row in rs) Console.WriteLine($"{row.GetValue<string>("pedido_id")} - {row.GetValue<decimal>("total")}");
    }
}