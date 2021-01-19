using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace playmongodb
{
    public class MongoTask
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }

        public static MongoTask Default => new MongoTask{
            Id = ObjectId.GenerateNewId(),
            Title = "Practice with MongoDB example",
            Deadline = DateTime.Now
        };
    }

    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "mongodb://jonatan:123456@localhost:27017";
            var databaseName = "playtask";
            MongoClient dbClient = new MongoClient(connectionString);
            
            var db = dbClient.GetDatabase(databaseName);

            var taskCollection = db.GetCollection<MongoTask>("tasks");

            taskCollection.InsertOne(MongoTask.Default);

            foreach (var item in taskCollection.AsQueryable())
            {
                System.Console.WriteLine($"TaskId - {item.Id}; {item.Title} ({item.Deadline})");
            }

            Console.WriteLine("Hello World!");
        }
    }
}
