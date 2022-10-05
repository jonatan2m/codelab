namespace DesignPatterns.Decorator.Example1
{
    /// <summary>
    /// Concrete Components provide default implementations of the operations.
    /// There might be several variations of these classes.
    /// </summary>
    public class ConcreteComponent : Component
    {
        public override string Operation()
        {
            return "ConcreteComponent";
        }
    }
}
