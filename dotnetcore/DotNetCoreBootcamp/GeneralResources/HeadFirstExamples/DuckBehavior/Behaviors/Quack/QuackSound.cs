using HeadFirstExamples.DuckBehavior.Contracts;

namespace HeadFirstExamples.DuckBehavior
{
    public class QuackSound : IQuackBehavior
    {
        public void Quack()
        {
            System.Console.WriteLine("Quack");
        }
    }
}
