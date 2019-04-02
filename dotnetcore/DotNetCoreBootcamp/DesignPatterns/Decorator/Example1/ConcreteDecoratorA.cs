namespace DesignPatterns.Decorator.Example1
{
    /// <summary>
    /// Concrete Decorators call the wrapped object and alter its result in some way.
    /// </summary>
    public class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(Component comp) : base(comp)
        {
        }

        /// <summary>
        /// Decorators may call parent implementation of the operation, instead of calling the wrapped object directly.
        /// This approach simplifies extension of decorator classes.
        /// </summary>
        /// <returns></returns>
        public override string Operation()
        {
            return $"ConcreteDecoratorA({base.Operation()})";
        }
    }
}
