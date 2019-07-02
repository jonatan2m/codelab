using HeadFirstExamples.DuckBehavior.Contracts;

namespace HeadFirstExamples.DuckBehavior
{
    public class Squeak : IQuackBehavior
    {
        public void Quack()
        {
            System.Console.WriteLine("Squeak");
        }
    }
}
