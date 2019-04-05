using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsConsole.ExamplesFromInternet.AvengersSimulatorHeuristic
{
    public class Character
    {
        public Character()
        { }

        public Character(Character character)
        {
            Name = character.Name;
            HP = character.HP;
            Attack = character.Attack;
            Defense = character.Defense;
            Score = character.Score;
        }

        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Score { get; set; }
        public bool IsAlive { get => HP > 0; }

        public void CauseDamage(int dano)
        {
            HP -= dano;
            HP = Math.Max(0, HP);
        }

        public static IEnumerable<Character> CreateAvengers()
        {
            yield return new Character()
            {
                Name = "Iron Man",
                Attack = 90,
                Defense = 100,
                HP = 250,
                Score = 150
            };

            yield return new Character()
            {
                Name = "Spider-Man",
                Attack = 85,
                Defense = 80,
                HP = 170,
                Score = 80
            };

            yield return new Character()
            {
                Name = "Nebula",
                Attack = 75,
                Defense = 80,
                HP = 150,
                Score = 50
            };

            yield return new Character()
            {
                Name = "Peter Quill",
                Attack = 80,
                Defense = 90,
                HP = 170,
                Score = 50
            };

            yield return new Character()
            {
                Name = "Drax",
                Attack = 80,
                Defense = 100,
                HP = 170,
                Score = 40
            };

            yield return new Character()
            {
                Name = "Mantis",
                Attack = 40,
                Defense = 40,
                HP = 100,
                Score = 10
            };

            yield return new Character()
            {
                Name = "Doctor Strange",
                Attack = 95,
                Defense = 80,
                HP = 170,
                Score = 120
            };
        }
        public static Character CreateThanos()
            => new Character()
            {
                Name = "Thanos",
                Attack = 240,
                Defense = 220,
                HP = 500,
                Score = 0
            };
    }
}
