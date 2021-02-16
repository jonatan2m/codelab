using System;

namespace Todolist.Web.Domain.Entities
{
    public class Todo
    {
        public string Title { get; set; }
        public bool Completed { get; set; }        
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}