using System;
namespace DesignPatterns.ChainOfResponsability.Example1
{
    /// <summary>
    /// Other approach
    /// https://stackoverflow.com/a/36495807/1604338
    /// </summary>
    public interface IChain
    {
        IChain Next { get; set; }
        void SetNext(IChain next);
        double Calculate(Numbers numbers);
    }
}
