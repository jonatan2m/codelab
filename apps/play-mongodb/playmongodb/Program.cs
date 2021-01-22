using System;
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
                Console.Clear();
                var taskCollection = GetCollection<MongoTask>();

                PrintTaskCollection(taskCollection);
                PrintCommands();

                var input = Console.ReadLine();

                var taskParts = input.Split(';');

                if (int.TryParse(taskParts[0], out int taskId))
                {
                    //edition or completion
                }
                else
                {
                    if (taskParts[0] == "_ft")
                    {
                        var filter = Builders<MongoTask>.Filter.Eq("Title", taskParts[1]);

                        var result = taskCollection.Find(filter).FirstOrDefault();
                        System.Console.WriteLine($"{result.Id} - {result.Title}");

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

        static IMongoCollection<T> GetCollection<T>()
        {
            var db = _dbClient.GetDatabase(DatabaseSettings.DatabaseName);

            return db.GetCollection<T>(MongoTask.CollectionName);
        }

        static void PrintTaskCollection(IMongoCollection<MongoTask> tasks)
        {
            const string tableFormat = "{0,30}{1,40}{2,50}";
            System.Console.WriteLine(tableFormat, "Id", "Name", "Deadline");

            foreach (var task in tasks.AsQueryable())
            {
                System.Console.WriteLine(tableFormat, task.Id, task.Title, task.Deadline);
            }
        }
    }
}
