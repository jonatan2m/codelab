using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace playmongodb
{

    class Program
    {
        static readonly MongoClient _dbClient;

        static Program()
        {
            _dbClient = new MongoClient(DatabaseSettings.ConnectionString);
        }

        static void PrintCommands()
        {
            System.Console.WriteLine("Create task: task_name;deadline");
            System.Console.WriteLine("Edit task: task_id;task_name");
            System.Console.WriteLine("Find tasks: _ft;title");
        }

        static void Main(string[] args)
        {
            while (true)
            {
                var taskCollection = GetCollection<MongoTask>(MongoTask.CollectionName);

                PrintTaskCollection(taskCollection);
                PrintCommands();

                var input = Console.ReadLine();
                Console.Clear();

                var taskParts = input.Split(';');

                if (int.TryParse(taskParts[0], out int taskId))
                {
                    //edition or completion
                }
                else
                {

                    if (taskParts[0] == "_ft")
                    {
                        //var filter = Builders<MongoTask>.Filter.Eq("Title", taskParts[1]);
                        //var tasks = taskCollection.Find(filter).FirstOrDefault();

                        var collection = GetCollection<MongoTask>(MongoTask.CollectionName);
                        var filter3 = Builders<MongoTask>.Filter.Regex("Title", new BsonRegularExpression($".*{taskParts[1]}.*"));
                        var tasks = collection.Find<MongoTask>(filter3).ToList();

                        foreach (var item in tasks)
                        {                            
                            System.Console.WriteLine($"{item.Id} - {item.Title}");
                        }

                        Console.ReadLine();
                        continue;
                    }
                    //find:
                    //creation
                    var task = new MongoTask
                    {
                        Id = DateTime.Now.Ticks,
                        Title = taskParts[0]
                    };

                    if (taskParts.Length > 1)
                    {
                        task.Deadline = DateTime.Parse(taskParts[1]);
                    }
                    else
                    {
                        task.Deadline = null;
                    }

                    taskCollection.InsertOne(task);
                }
            }
        }

        static IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var db = _dbClient.GetDatabase(DatabaseSettings.DatabaseName);

            return db.GetCollection<T>(collectionName);
        }

        static void PrintTaskCollection(IMongoCollection<MongoTask> tasks)
        {
            const string tableFormat = "{0,30}{1,40}{2,50}";
            System.Console.WriteLine(tableFormat, "Id", "Title", "Deadline");

            /* Another way to read all documents
            var documents = tasks.Find(new MongoDB.Bson.BsonDocument()).ToList();

            foreach (var task in documents)
            {
                System.Console.WriteLine(tableFormat, task.Id, task.Title, task.Deadline);
            }
            */

            foreach (var task in tasks.AsQueryable())
            {
                System.Console.WriteLine(tableFormat, task.Id, task.Title, task.Deadline);
            }
        }
    }
}
