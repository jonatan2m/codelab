using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.Graphs.Representations
{   
    public record Person (string Name);

    /// <summary>
    /// First, set the number of vertices to build the matrix
    /// </summary>
    public class SocialGraphAdjacencyMatrix
    {
        private bool[,] _matrix;
        private List<Person> _people;
        public SocialGraphAdjacencyMatrix(List<Person> people)
        {
            _people= people;
            _matrix = new bool[people.Count, people.Count];
        }

        public void AddConnection(Person person1, Person person2)
        {
            int p1 = _people.IndexOf(person1);
            int p2 = _people.IndexOf(person2);
            _matrix[p1, p2] = true;
            _matrix[p2, p1] = true;
        }

        public void RemoveConnection(Person person1, Person person2)
        {
            int p1 = _people.IndexOf(person1);
            int p2 = _people.IndexOf(person2);
            _matrix[p1, p2] = false;
            _matrix[p2, p1] = false;
        }

        public bool AreConnected(Person person1, Person person2) 
        {
            int p1 = _people.IndexOf(person1);
            int p2 = _people.IndexOf(person2);

            return _matrix[p1, p2];
        }
    }

    public class SocialGraphAdjacencyMatrixTests
    {
        [Fact]
        public void Test1()
        {
            List<Person> people = new List<Person>(3)
            {
                new Person("João"),
                new Person("Maria"),
                new Person("Carla")
            };
            var graph = new SocialGraphAdjacencyMatrix(people);

            graph.AddConnection(people[0], people[1]);
            graph.AddConnection(people[2], people[1]);

            Assert.True(graph.AreConnected(people[0], people[1]));
            Assert.True(graph.AreConnected(people[2], people[1]));
            Assert.False(graph.AreConnected(people[2], people[0]));
        }        
    }
}
