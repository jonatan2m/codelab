using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using AppSeed.Providers;

var config = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "appsettings.json"), optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("== App.Seed ==");
var sw = Stopwatch.StartNew();

// Mongo
try { await MongoOps.SeedAsync(config); } catch (Exception ex) { Warn("Mongo", ex); }
// Redis
try { await RedisOps.SeedAsync(config); } catch (Exception ex) { Warn("Redis", ex); }
// Cassandra
try { await CassandraOps.SeedAsync(config); } catch (Exception ex) { Warn("Cassandra", ex); }
// Neo4j
try { await Neo4jOps.SeedAsync(config); } catch (Exception ex) { Warn("Neo4j", ex); }
// Elasticsearch
try { await ElasticOps.SeedAsync(config); } catch (Exception ex) { Warn("Elasticsearch", ex); }
// Influx
try { await InfluxOps.SeedAsync(config); } catch (Exception ex) { Warn("Influx", ex); }

sw.Stop();
Console.WriteLine($"\nSeed concluído em {sw.ElapsedMilliseconds} ms");
return;

static void Warn(string name, Exception ex)
{
    Console.WriteLine($"[SKIP] {name}: {ex.GetType().Name} - {ex.Message}");
}
