using HeadFirstExamples.DuckBehavior.Contracts;

namespace HeadFirstExamples.DuckBehavior
{
    public class MuteQuack : IQuackBehavior
    {
        public void Quack()
        {
            System.Console.WriteLine("<< Silence >>");
        }
    }
}
