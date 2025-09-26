using Microsoft.Extensions.Configuration;
using AppDemo.Providers;

var config = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "appsettings.json"), optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("== App.Demo ==");

// Cada bloco tenta rodar apenas se o serviço estiver disponível
try { await MongoOps.RunAsync(config); } catch (Exception ex) { Skip("Mongo", ex); }
try { await RedisOps.RunAsync(config); } catch (Exception ex) { Skip("Redis", ex); }
try { await CassandraOps.RunAsync(config); } catch (Exception ex) { Skip("Cassandra", ex); }
try { await Neo4jOps.RunAsync(config); } catch (Exception ex) { Skip("Neo4j", ex); }
try { await ElasticOps.RunAsync(config); } catch (Exception ex) { Skip("Elastic", ex); }
try { await InfluxOps.RunAsync(config); } catch (Exception ex) { Skip("Influx", ex); }

static void Skip(string name, Exception ex) =>
  Console.WriteLine($"[SKIP] {name}: {ex.GetType().Name} - {ex.Message}");
