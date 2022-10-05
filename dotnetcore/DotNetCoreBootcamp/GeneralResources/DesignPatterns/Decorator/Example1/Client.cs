namespace DesignPatterns.Decorator.Example1
{
    public class Client
    {
        // The client code works with all objects using the Component interface.
        // This way it can stay independent of the concrete classes of
        // components it works with.
        public void ClientCode(Component component)
        {
            System.Console.WriteLine("RESULT: " + component.Operation());
        }
    }
}
