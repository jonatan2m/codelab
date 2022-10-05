using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.LINQ
{
    public class GroupByExample
    {
        private class Todo
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int Category { get; set; }
        }

        private class TodoSummary
        {
            public string Item { get; set; }
        }

        public static void Run()
        {
            var todo1 = Enumerable.Repeat(new Todo {Category = 1, Id = 1, Text = "1"}, 5);
            var todo2 = Enumerable.Repeat(new Todo {Category = 2, Id = 2, Text = "2"}, 5);

            var result = todo1.Concat(todo2).GroupBy(x => new {x.Category})
                .Select(x => new TodoSummary
                {
                    Item = $"{x.Key.Category}:Items"
                });

            List<TodoSummary> resultList = result.ToList();

            foreach (var todo in resultList)
            {
                Console.WriteLine(todo.Item);
            }
        }
    }
}
