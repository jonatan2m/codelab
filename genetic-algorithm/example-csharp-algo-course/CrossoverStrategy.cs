// See https://aka.ms/new-console-template for more information

using System.Collections;

public interface ICrossoverStrategy
{
    System.Collections.BitArray Crossover(
        System.Collections.BitArray parent1,
        System.Collections.BitArray parent2);
}

//Essa estratégia divide a montagem do filho
//Uma parte a esquerda do ponto, vem de um pai e a direita do outro pai
public class SinglePointCrossoverStrategy : ICrossoverStrategy
{
    private Random _random;

    public SinglePointCrossoverStrategy()
    {
         _random = new Random();
    }

    public BitArray Crossover(BitArray parent1, BitArray parent2)
    {
        var crossoverPoint = _random.Next(parent1.Count);
        var child = new BitArray(parent1.Count);
        for (int i = 0; i < crossoverPoint; i++)
        {
            child[i] = parent1[i];
        }

        for (int i = crossoverPoint; i < parent2.Count; i++)
        {
            child[i] = parent2[i];
        }

        return child;
    }
}

public class UniformCrossoverStrategy : ICrossoverStrategy
{
    private Random _random;

    public UniformCrossoverStrategy()
    {
        _random = new Random();
    }

    public BitArray Crossover(BitArray parent1, BitArray parent2)
    {
        var child = new BitArray(parent1.Count);

        for (int i = 0; i < parent1.Count; i++)
        {
            child[i] = _random.NextDouble() < 0.5 ? parent1[i] : parent2[i];
        }

        return child;
    }
}