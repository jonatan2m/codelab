using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;

namespace AppDemo.Providers;

public static class ElasticOps
{
    public static async Task RunAsync(IConfiguration cfg)
    {
        var es = new ElasticsearchClient(new ElasticsearchClientSettings(new Uri(cfg["Elasticsearch:Uri"]!)));
        var index = cfg["Elasticsearch:Index"] ?? "produtos";

        var search = await es.SearchAsync<object>(s => s
            .Index(index)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(new[] { "nome^2", "descricao" })
                    .Query("notebook")
                )));

        Console.WriteLine($"Elastic: hits={search.Hits.Count}");
    }
}