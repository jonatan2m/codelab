// See https://aka.ms/new-console-template for more information

public interface IParentSelectionStrategy
{
    Solution SelectParent(Population population);
}

public class TournamentSelectionStrategy : IParentSelectionStrategy
{
    private readonly Random _random;
    private readonly int _tournamentSize;
    private readonly IComparer<Solution> _fitnessComparer;

    public TournamentSelectionStrategy(
        int tournamentSize, IComparer<Solution> fitnessComparer)
    {
        _random = new Random();
        _tournamentSize = tournamentSize;
        _fitnessComparer = fitnessComparer;
    }

    public Solution SelectParent(Population population)
    => Enumerable.Range(0, _tournamentSize)
        .Select(_ => population.Solutions[_random.Next(population.Solutions.Count)])
        .Max(_fitnessComparer);
}

public class RouletteWheelSelectionStrategy : IParentSelectionStrategy
{
    private readonly Random _random;

    public RouletteWheelSelectionStrategy()
    {
        _random = new Random();
    }

    public Solution SelectParent(Population population)
    {
        var totalFitness = population.Solutions
        .Select(s => s.TotalValue).Sum();
        var roulettValue = (decimal)(_random.NextDouble() * (double)totalFitness);
        var partialSum = 0M;
        for (int i = 0; i < population.Solutions.Count; i++)
        {
            partialSum += population.Solutions[i].TotalValue;
            if (partialSum >= roulettValue)
                return population.Solutions[i];
        }
        return population.Solutions.Last();

    }
}

