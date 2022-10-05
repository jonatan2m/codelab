namespace DesignPatterns.Composite.GiftExample
{
    /// <summary>
    /// Somente o Composite vai implementar, os Leafs não, mas por um questão de divisão das responsabilidades
    /// </summary>
    public interface IGiftOperations
    {
        void Add(GiftBase gift);
        void Remove(GiftBase gift);
    }
}