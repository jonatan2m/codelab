using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEventsTest.SharingContext
{
    public class Player
    {
        public readonly string Name;
        public int Health { get; private set; }

        public Player(string name)
        {
            Name = name;
            Health = new Random().Next(50, 100);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }

    public class GameState
    {
        public IList<Player> Players { get; set; } = new List<Player>();

        public Guid Id { get; } = Guid.NewGuid();

        public const int EarthquakeDamage = 25;

        public GameState()
        {
            CreateNewWorld();
        }

        public void Reset()
        {
            Players.Clear();
        }

        public void Earthquake()
        {
            foreach (var player in Players)
            {
                player.TakeDamage(EarthquakeDamage);
            }
        }

        private void CreateNewWorld()
        {
            //expensive creation
            System.Threading.Thread.Sleep(2000);
        }
    }
}
