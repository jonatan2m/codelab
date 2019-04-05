using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatternsConsole.ExamplesFromInternet.AvengersSimulatorHeuristic
{
    /// <summary>
    /// https://github.com/gabrielschade/algorithms/tree/master/AvengersSimulatorHeuristic
    /// </summary>
    public class PlayAvengers
    {
        public static void Run()
        {
            var avengers = Character.CreateAvengers().ToList();
            var thanos = Character.CreateThanos();

            Turn initial = new Turn(
                thanos,
                avengers,
                0,
                0,
                null);

            BattleSimulator battleSimulator = new BattleSimulator(
                initial,
                (allPossibleTurns, currentTurn) =>
                currentTurn == allPossibleTurns.OrderByDescending(turn => turn.BattleScore)
                                               .First()
                );

            battleSimulator.GeneratePossibleTurns(initial, 1, 0);

            Console.WriteLine($"Victories: {battleSimulator.Victories.Count}");
            Console.WriteLine($"Defeats: {battleSimulator.Defeats.Count}");

            //PrintBattle(battleSimulator.Initial);
            Console.ReadKey();

        }

        private static void PrintBattle(Turn initialTurn)
        {
            Console.WriteLine(initialTurn.Description);

            if (!initialTurn.BattleIsOver)
                PrintBattle(initialTurn.NextPossibleTurns.FirstOrDefault());
        }
    }
}
