// See https://aka.ms/new-console-template for more information

public record Item(string Name, double Weight, decimal Value);
public record Knapsack(double MaxWeight);

public class Problem
{
    public IReadOnlyList<Item> Items { get; }
    public Knapsack Knapsack { get; }

    public Problem(IEnumerable<Item> items, Knapsack knapsack)
    {
        Items = items.ToList();
        Knapsack = knapsack;
    }

    //Cria um exemplo do problema com 15 itens e uma mochila
    public static Problem CreateSample()
    {
        var random = new Random();
        var items = Enumerable.Range(1, 300)
            .Select(i => new Item($"Item {i}", random.Next(1, 101), random.Next(1, 50)))
            .ToList();
        var knapsack = new Knapsack(1000);//1 kg, a unidade é grama
        return new Problem(items, knapsack);
    }
}