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

        static void Main(string[] args)
        {
            System.Console.WriteLine("Create task: task_name;deadline");
            System.Console.WriteLine("Edit task: task_id;task_name");


            while (true)
            {
                Console.Clear();
                var taskCollection = GetCollection<MongoTask>();

                //taskCollection.InsertOne(MongoTask.Default);
                
                PrintTaskCollection(taskCollection);                

                Console.ReadLine();
            }
        }

        static IMongoCollection<T> GetCollection<T>()
        {
            var db = _dbClient.GetDatabase(DatabaseSettings.DatabaseName);

            return db.GetCollection<T>(MongoTask.CollectionName);
        }

        static void PrintTaskCollection(IMongoCollection<MongoTask> tasks)
        {
            string tableFormat = "{0,30}{1,40}{2,50}";
            System.Console.WriteLine(tableFormat, "Id", "Name", "Deadline");
            
            foreach (var task in tasks.AsQueryable())
            {
                System.Console.WriteLine(tableFormat, task.Id, task.Title, task.Deadline);
            }
        }
    }
}
