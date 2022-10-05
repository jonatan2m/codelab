namespace DesignPatterns.Decorator.Example1
{
    /// <summary>
    /// Decorators can execute their behavior either before or after the call to a wrapped object.
    /// </summary>
    public class ConcreteDecoratorB : Decorator
    {
        public ConcreteDecoratorB(Component comp) : base(comp)
        {
        }

        public override string Operation()
        {
            return $"ConcreteDecoratorB({base.Operation()})";
        }
    }
}
