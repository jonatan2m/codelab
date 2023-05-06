// See https://aka.ms/new-console-template for more information

using System.Collections;

public interface IMutationStrategy
{
    BitArray Mutate(BitArray solution, double mutationRate);
}

public class SimpleMutationStrategy : IMutationStrategy
{
    private Random _random;

    public SimpleMutationStrategy()
    {
        _random = new Random();
    }

    public BitArray Mutate(BitArray solution, double mutationRate)
    {
        BitArray result = new(solution);
        for (int i = 0; i < result.Count; i++)
        {
            if(_random.NextDouble() < mutationRate)
                result[i] = !solution[i];
        }

        return result;
    }
}