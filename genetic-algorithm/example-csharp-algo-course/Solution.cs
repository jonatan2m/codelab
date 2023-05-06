// See https://aka.ms/new-console-template for more information
public class Solution
{
    public Problem Problem { get; }
    public System.Collections.BitArray Genes { get; set; }

    public int ItemsCount { get; private set; }
    public double TotalWeight { get; private set; }
    public decimal TotalValue { get; private set; }

    public Solution(Problem problem, System.Collections.BitArray genes)
    {
        Problem = problem;
        Genes = genes;

        for (int i = 0; i < genes.Count; i++)
        {
            //faz uma verificação para impedir que seja colocado mais peso do que a mochila aguenta
            //vale destacar que aqui nem todos os itens estão sendo observados e essa é a graça do algoritmo genético.
            //a melhor solução se dará por evolução, cruzando e mutação
            if(genes[i] &&
             ((problem.Items[i].Weight + TotalWeight) < problem.Knapsack.MaxWeight))
            {
                TotalWeight += problem.Items[i].Weight;
                TotalValue += problem.Items[i].Value;
                ItemsCount++;
            }
        }
    }

    public override string ToString()
        => $"{ItemsCount} item(s). \t Weight: {TotalWeight} \t Value: {TotalValue}";

    public IEnumerable<Item> Items
    {
        get 
        {
            var weight = 0.0;
            for (int i = 0; i < Genes.Count; i++)
            {
                if(Genes[i] &&
                ((Problem.Items[i].Weight + weight) < Problem.Knapsack.MaxWeight))                
                {
                    yield return Problem.Items[i];
                    weight += Problem.Items[i].Weight;
                }
            }
        }
    }
}