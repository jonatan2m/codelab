using MongoDB.Driver;
using AutoFixture;

namespace playmongodb
{

    public class SeedDataProvider
    {       
        readonly IMongoCollection<MongoTask> _collection;

        public SeedDataProvider(string connectionString, string databaseName)
        {
            var dbClient = new MongoClient(connectionString);
            //var databaseName = new MongoUrl(connectionString).DatabaseName;
            
            var database = dbClient.GetDatabase(databaseName);
            _collection = database.GetCollection<MongoTask>(MongoTask.CollectionName);
        }

        public void SeedData()
        {
            var fixture = new Fixture();
            
            var tasks = fixture.Build<MongoTask>()
            .Without(x => x.Id)
            .CreateMany(10);

            _collection.InsertMany(tasks);            
        }
    }
}