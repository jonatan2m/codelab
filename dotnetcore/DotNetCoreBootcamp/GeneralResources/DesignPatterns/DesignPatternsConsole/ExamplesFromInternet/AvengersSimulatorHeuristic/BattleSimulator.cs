using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatternsConsole.ExamplesFromInternet.AvengersSimulatorHeuristic
{
    public class BattleSimulator
    {
        public Turn Initial { get; }
        public List<Turn> Victories { get; }
        public List<Turn> Defeats { get; }
        public int Total { get => Victories.Count + Defeats.Count; }

        public Func<List<Turn>, Turn, bool> EvaluationFunction { get; }

        public BattleSimulator(Turn initial,
            Func<List<Turn>, Turn, bool> evaluationFunction)
        {
            Initial = initial;
            EvaluationFunction = evaluationFunction;
            Victories = new List<Turn>();
            Defeats = new List<Turn>();
        }

        private IEnumerable<Turn> GeneratePossibleTurnsFromThanos(Turn currentTurn, int turnNumber)
        {
            for (int avengerIndex = 0;
                avengerIndex < currentTurn.Avengers.Count;
                avengerIndex++)
                if (currentTurn.Avengers[avengerIndex].IsAlive)
                {
                    var turn = new Turn(currentTurn.Thanos,
                                            currentTurn.Avengers,
                                            turnNumber,
                                            avengerIndex,
                                            currentTurn);

                    turn.CalculateBattle(turn.Thanos,
                            turn.Avenger, 2, 4);

                    yield return turn;
                }
        }

        private IEnumerable<Turn> GeneratePossibleTurnsFromAvengers(Turn currentTurn, int turnNumber, int avengerIndex)
            => Enumerable.Range(1, 3)
                .Select(bonus =>
                {
                    var turn = new Turn(currentTurn.Thanos,
                               currentTurn.Avengers,
                               turnNumber,
                               avengerIndex,
                               currentTurn);
                    turn.CalculateBattle(turn.Avenger,
                                     turn.Thanos,
                                     bonus,
                                     1);
                    return turn;
                });

        private void UpdateValuesOfBattle(Turn endTurn)
        {
            if (endTurn.Thanos.HP > 0)
                Defeats.Add(endTurn);
            else
                Victories.Add(endTurn);
        }

        public void GeneratePossibleTurns(
            Turn currentTurn,
            int turnNumber,
            int avengerIndex)
        {
            bool isThanosTurn = turnNumber % 2 == 0;
            IEnumerable<Turn> generatedTurns =
                isThanosTurn ?
                GeneratePossibleTurnsFromThanos(currentTurn, turnNumber)
                : GeneratePossibleTurnsFromAvengers(currentTurn, turnNumber, avengerIndex);

            List<Turn> futureTurns = ApplyEvaluateFunction(generatedTurns.ToList());
            currentTurn.NextPossibleTurns.AddRange(futureTurns);

            foreach (var nextPossibleTurn in currentTurn.NextPossibleTurns)
            {
                if (nextPossibleTurn.BattleIsOver)
                    UpdateValuesOfBattle(nextPossibleTurn);
                else
                    GeneratePossibleTurns(
                        nextPossibleTurn,
                        turnNumber + 1,
                        GetNextAvenger(avengerIndex, isThanosTurn, nextPossibleTurn));
            }
        }

        private List<Turn> ApplyEvaluateFunction(List<Turn> generatedTurns)
        {
            Defeats.AddRange(generatedTurns.Where(turn => !EvaluationFunction(generatedTurns, turn)));
            return generatedTurns.Where(turn => EvaluationFunction(generatedTurns, turn))
                                .ToList();
        }

        private int GetNextAvengerIndex(int avenger)
        {
            avenger++;
            if (avenger > Initial.Avengers.Count - 1)
                avenger = 0;
            return avenger;
        }

        private int GetNextAvenger(int avenger, bool isThanosTurn, Turn currentTurn)
        {
            int nextAvenger =
                isThanosTurn ?
                    avenger
                    : GetNextAvengerIndex(avenger);

            while (!currentTurn.Avengers[nextAvenger].IsAlive)
                nextAvenger = GetNextAvengerIndex(nextAvenger);

            return nextAvenger;
        }
    }
}
