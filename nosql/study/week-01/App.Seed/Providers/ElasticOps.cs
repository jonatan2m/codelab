using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace AppSeed.Providers;

public static class ElasticOps
{
    public static async Task SeedAsync(IConfiguration cfg)
    {
        var uri = cfg["Elasticsearch:Uri"]!;
        var index = cfg["Elasticsearch:Index"] ?? "produtos";

        Console.WriteLine("Elastic: conectando...");
        var settings = new ElasticsearchClientSettings(new Uri(uri))
            .ThrowExceptions(alwaysThrow: true);
        var es = new ElasticsearchClient(settings);

        if (!(await es.Indices.ExistsAsync(index)).Exists)
            await es.Indices.CreateAsync(index);

        var doc = new { nome = "Notebook Gamer", descricao = "RTX 4060, 16GB RAM", preco = 7999.0 };
        await es.IndexAsync(doc, idx => idx.Index(index));

        Console.WriteLine("Elastic: seed ok");
    }
}
