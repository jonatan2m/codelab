using System;

namespace HeadFirstExamples.DuckBehavior.Ducks
{
    public class Mallard : Duck
    {
        public Mallard()
            :base(new FlyWithWings(), new QuackSound())
        {
        }

        public override void Display()
        {
            Console.WriteLine("I'm a mallard duck");
        }
    }
}
