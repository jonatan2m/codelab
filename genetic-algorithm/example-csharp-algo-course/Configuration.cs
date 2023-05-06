// See https://aka.ms/new-console-template for more information
public class Configuration
{
    static Lazy<Configuration>  s_default => new(() => {
        var fitnessComparer = new SolutionValueAndWeightComparer();
        return new Configuration(populationSize: 20,
                                 maxGenerations: 100,
                                 crossoverRate: 0.8,
                                 initialMutationRate: 0.1,
                                 initialSolutionRate: 0.5,
                                 fitnessComparer,
                                 parentSelectionStrategy: new TournamentSelectionStrategy(5, fitnessComparer),
                                 crossoverStrategy: new UniformCrossoverStrategy(),
                                 mutationStrategy: new SimpleMutationStrategy());
    });

    public Configuration(
        int populationSize,
        int maxGenerations,
        double crossoverRate,
        double initialMutationRate,
        double initialSolutionRate,
        IComparer<Solution> fitnessComparer,
        IParentSelectionStrategy parentSelectionStrategy,
        UniformCrossoverStrategy crossoverStrategy,
        SimpleMutationStrategy mutationStrategy)
    {
        PopulationSize = populationSize;
        MaxGenerations = maxGenerations;
        CrossoverRate = crossoverRate;
        MutationRate = initialMutationRate;
        InitialSolutionRate = initialSolutionRate;
        FitnessComparer = fitnessComparer;
        ParentSelectionStrategy = parentSelectionStrategy;
        CrossoverStrategy = crossoverStrategy;
        MutationStrategy = mutationStrategy;
    }
    public static Configuration Default => s_default.Value;

    public double MutationRate { get; internal set; }
    public int MaxGenerations { get; internal set; }
    public double CrossoverRate { get; }   
    public IComparer<Solution> FitnessComparer { get; internal set; }
    public IParentSelectionStrategy ParentSelectionStrategy { get; }
    public UniformCrossoverStrategy CrossoverStrategy { get; }
    public SimpleMutationStrategy MutationStrategy { get; }
    public int PopulationSize { get; internal set; }
    public double InitialSolutionRate { get; internal set; }
}