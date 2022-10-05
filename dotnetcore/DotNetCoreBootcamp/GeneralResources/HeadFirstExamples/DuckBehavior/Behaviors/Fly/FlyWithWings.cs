using HeadFirstExamples.DuckBehavior.Contracts;
using System;

namespace HeadFirstExamples.DuckBehavior
{
    public class FlyWithWings : IFlyBehavior
    {
        public void Fly()
        {
            Console.WriteLine("I'm flying!");
        }
    }

    public class FlyRocketPowered : IFlyBehavior
    {
        public void Fly()
        {
            Console.WriteLine("I'm flying with a rocket!");
        }
    }
}
