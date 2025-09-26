using Microsoft.Extensions.Configuration;
using Neo4j.Driver;

namespace AppDemo.Providers;

public static class Neo4jOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        await using var driver = GraphDatabase.Driver(cfg["Neo4j:Uri"], AuthTokens.Basic(cfg["Neo4j:User"], cfg["Neo4j:Password"]));
        await using var session = driver.AsyncSession();

        var result = await session.RunAsync(@"
            MATCH (a:User{id:'u1'})-[:SEGUE]->(x)<-[:SEGUE]-(b:User{id:'u2'})
            RETURN x.nome as nome");

        Console.WriteLine("Neo4j: amigos em comum (u1,u2)");
        await result.ForEachAsync(r => Console.WriteLine(r["nome"].As<string>()));
    }
}