namespace DesignPatterns.Interpreter
{
    public abstract class Spec
    {
        public abstract bool IsSatisfiedBy(Product product);
    }
}