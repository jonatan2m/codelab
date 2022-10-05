using HeadFirstExamples.DuckBehavior.Contracts;
using HeadFirstExamples.DuckBehavior.Ducks;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeadFirstExamples.DuckBehavior
{
    public abstract class Duck
    {
        IFlyBehavior _flyBehavior;
        IQuackBehavior _quackBehavior;

        public Duck(IFlyBehavior flyBehavior, IQuackBehavior quackBehavior)
        {
            _flyBehavior = flyBehavior;
            _quackBehavior = quackBehavior;
        }

        public abstract void Display();

        public void PerformFly() { _flyBehavior.Fly(); }

        public void PerformQuack() { _quackBehavior.Quack(); }

        public void Swim() { Console.WriteLine("All duck float, even decoys!"); }

        public void SetFlyBehavior(IFlyBehavior flyBehavior)
        {
            _flyBehavior = flyBehavior;
        }
    }

    public static class DuckPlay
    {
        public static void Play()
        {
            Duck mallard = new Mallard();
            mallard.PerformQuack();
            mallard.PerformFly();

            Duck model = new ModelDuck();
            model.PerformFly();
            model.SetFlyBehavior(new FlyRocketPowered());
            model.PerformFly();
        }
    }
}
