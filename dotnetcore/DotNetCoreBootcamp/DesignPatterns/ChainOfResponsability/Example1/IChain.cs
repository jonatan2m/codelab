using System;
namespace DesignPatterns.ChainOfResponsability.Example1
{
    public interface IChain
    {
        IChain Next { get; set; }
        void SetNext(IChain next);
        double Calculate(Numbers numbers);
    }
}
