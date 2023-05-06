// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var problem = Problem.CreateSample();
var configuration = Configuration.Default;

var finalSolution = GeneticAlgorithm.Run(problem, configuration);

//Essa solução final contém todas as configurações, dentre os 300 itens do exemplo.
//Pra obter os itens exatos, é necessário ir pegando os itens até a capacidade da mochila.
//O que amezina isso é que a solução contém o número de items, então seria só pegar o número de itens retornados.
System.Console.WriteLine($"Final solution: {finalSolution}");

public class GeneticAlgorithm
{
    public static Solution Run(
        Problem problem,
        Configuration configuration)
    {
        var population = new Population(problem, configuration);
        Solution bestSolution = population.GetBestSolution();

        System.Console.WriteLine($"First Generation: {bestSolution}");
        var initialMutationRate = configuration.MutationRate;
        var staleGenerationsCount = 0;

        for (int i = 0; i < configuration.MaxGenerations; i++)
        {
            population = population.Evolve();
            var candidate = population.GetBestSolution();

            if (configuration.FitnessComparer
            .Compare(bestSolution, candidate) == 0)
            {
                System.Console.WriteLine(
                    $"Generation {i + 1}: No improvements, incrementing mutation"
                );
                staleGenerationsCount++;
                configuration.MutationRate = initialMutationRate * (Math.Pow(1.1, staleGenerationsCount));
            }
            else
            {
                bestSolution = candidate;
                configuration.MutationRate = initialMutationRate;
                staleGenerationsCount = 0;
                System.Console.WriteLine($"Generation {i + 1}: {bestSolution}");
            }
        }
        return bestSolution;
    }
}