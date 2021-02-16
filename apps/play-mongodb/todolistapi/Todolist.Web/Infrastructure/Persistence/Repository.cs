using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Todolist.Web.Domain.Persistence.Repository;

namespace Todolist.Web.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T>
    {
        public Repository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("mongodblocal");

            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;

            _database = client.GetDatabase(databaseName);


        }
        
        public IMongoCollection<T> Collection => throw new System.NotImplementedException();

        protected IMongoDatabase _database;
        public IMongoDatabase Database => _database;
    }
}