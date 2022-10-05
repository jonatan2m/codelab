namespace DesignPatterns.Interpreter
{
    public class AndSpec : Spec
    {
        public Spec Augend { get; private set; }
        public Spec Addend { get; private set; }
        public AndSpec(Spec augend, Spec addend)
        {
            Augend = augend;
            Addend = addend;
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return Augend.IsSatisfiedBy(product) &&
                   Addend.IsSatisfiedBy(product);
        }
    }
}