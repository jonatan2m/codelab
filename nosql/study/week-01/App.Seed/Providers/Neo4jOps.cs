using Microsoft.Extensions.Configuration;
using Neo4j.Driver;

namespace AppSeed.Providers;

public static class Neo4jOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        var uri = cfg["Neo4j:Uri"]!;
        var user = cfg["Neo4j:User"]!;
        var pass = cfg["Neo4j:Password"]!;

        Console.WriteLine("Neo4j: conectando...");
        await using var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, pass));
        await using var session = driver.AsyncSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("CREATE (:User {id:'u1',nome:'Ana'})");
            await tx.RunAsync("CREATE (:User {id:'u2',nome:'Bruno'})");
            await tx.RunAsync("MATCH (a:User{id:'u1'}),(b:User{id:'u2'}) CREATE (a)-[:SEGUE]->(b)");
        });

        Console.WriteLine("Neo4j: seed ok");
    }
}
