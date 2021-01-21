using System;
using MongoDB.Bson;

namespace playmongodb
{
    public class MongoTask
    {
        public static readonly string CollectionName = "tasks";
        // public ObjectId Id { get; set; }
        public int Id { get; set; }
        
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }

        public static MongoTask Default => new MongoTask
        {
            //Id = ObjectId.Parse("1f41f41f41f41f41f41f41f4"),
            //Id = ObjectId.GenerateNewId(),
            Title = "Practice with MongoDB example",
            Deadline = DateTime.Now
        };
    }
}
