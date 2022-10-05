using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatternsConsole.ExamplesFromInternet.AvengersSimulatorHeuristic
{
    public class Turn
    {
        public Turn()
        {
            NextPossibleTurns = new List<Turn>();
        }

        public Turn(
            Character thanos,
            List<Character> avengers,
            int number,
            int avenger,
            Turn previousTurn)
        {
            Thanos = new Character(thanos);
            NextPossibleTurns = new List<Turn>();
            Number = number;
            PreviousTurn = previousTurn;

            Avengers =
                JsonConvert.DeserializeObject<List<Character>>(
                    JsonConvert.SerializeObject(avengers));
            Avenger = Avengers[avenger];
        }

        public Character Thanos { get; set; }
        public Character Avenger { get; set; }
        public List<Character> Avengers { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }

        public List<Turn> NextPossibleTurns { get; set; }
        public Turn PreviousTurn { get; set; }

        public bool BattleIsOver
        {
            get => !Thanos.IsAlive
                   || Avengers.All(avenger => !avenger.IsAlive);
        }

        public int Score
        {
            get => BattleScore - PreviousTurn.BattleScore;
        }

        public int BattleScore
        {
            get => Avengers.Where(avenger => avenger.IsAlive)
                             .Sum(avenger => avenger.Score)
                   - Thanos.HP;
        }

        public void CalculateBattle(
            Character ofensiveBattler,
            Character defensiveBattler,
            int attackBonus,
            int defenseBonus)
        {
            int attack = ofensiveBattler.Attack * attackBonus;
            int defense = defensiveBattler.Defense * defenseBonus;
            int damage = Math.Max(attack - defense, 0);
            defensiveBattler.CauseDamage(damage);
            string dead =
                defensiveBattler.IsAlive ?
                string.Empty
                : $"{Environment.NewLine}{defensiveBattler.Name} is dead.";

            Description = $"{ofensiveBattler.Name} attacked {defensiveBattler.Name} dealing {damage} damage.{dead}";
        }
    }
}
