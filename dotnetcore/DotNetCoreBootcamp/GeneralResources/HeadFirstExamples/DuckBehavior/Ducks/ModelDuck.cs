using System;
using System.Collections.Generic;
using System.Text;

namespace HeadFirstExamples.DuckBehavior.Ducks
{
    public class ModelDuck : Duck
    {
        public ModelDuck()
            :base(new FlyNoWay(), new QuackSound())
        {

        }

        public override void Display()
        {
            Console.WriteLine("I'm a model duck");
        }
    }
}
