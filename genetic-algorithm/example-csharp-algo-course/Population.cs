// See https://aka.ms/new-console-template for more information
public class Population
{
    private Problem _problem;
    private readonly Random _random;
    private Configuration _configuration;
    public IReadOnlyList<Solution> Solutions { get; private set; }

    public Population(Problem problem, Configuration configuration)
    {
        _random = new Random();
        _problem = problem;
        _configuration = configuration;

        Solutions = Enumerable.Range(0, configuration.PopulationSize)
        .Select(_ => GenerateRandomSolution())
        .ToList();
    }

    public Population(
        Problem problem,
        Configuration configuration,
        IEnumerable<System.Collections.BitArray> setups)
    {
        _random = new Random();
        _problem = problem;
        _configuration = configuration; 
        Solutions = setups
            .Select(g => new Solution(problem, g))
            .ToList();
    }

    private Solution GenerateRandomSolution()
    {
        //representa todos os itens que podem estar na mochila
        var genes = new System.Collections.BitArray(_problem.Items.Count);
        for (int i = 0; i < genes.Count; i++)
        {
            genes[i] = _random.NextDouble() < _configuration.InitialSolutionRate;
        }

        return new Solution(_problem, genes);
    }

    internal Population Evolve()
    {
        var newSetups = new List<System.Collections.BitArray>();

        //vai pegar as 10% melhores soluções.
        var bestSolutions = GetBestSolutions(_configuration.PopulationSize / 10);
        newSetups.AddRange(bestSolutions.Select(s => s.Genes));

        while(newSetups.Count < _configuration.PopulationSize)
        {
            var parent1 = _configuration.ParentSelectionStrategy.SelectParent(this);
            var parent2 = _configuration.ParentSelectionStrategy.SelectParent(this);
            var child = _random.NextDouble() < _configuration.CrossoverRate?
                _configuration
                .CrossoverStrategy.Crossover(parent1.Genes, parent2.Genes):
                parent1.Genes;
            child = _configuration.MutationStrategy
                .Mutate(child, _configuration.MutationRate);
            
            newSetups.Add(child);
        }

        return new Population(_problem, _configuration, newSetups);

    }

    private IEnumerable<Solution> GetBestSolutions(int count)
    => Solutions
    .OrderByDescending(s => s, _configuration.FitnessComparer)
    .Take(count);

    internal Solution GetBestSolution()
    {
        return Solutions.Max(_configuration.FitnessComparer);
    }
}