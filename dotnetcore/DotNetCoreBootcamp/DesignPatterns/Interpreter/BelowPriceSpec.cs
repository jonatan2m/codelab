namespace DesignPatterns.Interpreter
{
    public class BelowPriceSpec : Spec
    {
        public float Price { get; private set; }

        public BelowPriceSpec(float price)
        {
            Price = price;
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return product.Price < Price;
        }
    }
}