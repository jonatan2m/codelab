using HeadFirstExamples.DuckBehavior.Contracts;
using System;

namespace HeadFirstExamples.DuckBehavior
{
    public class FlyNoWay : IFlyBehavior
    {
        public void Fly()
        {
            Console.WriteLine("I can't fly!");
        }
    }
}
