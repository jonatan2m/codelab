using System.Drawing;

namespace DesignPatterns.Interpreter
{
    public class ColorSpec : Spec
    {
        public Color Color { get; private set; }

        public ColorSpec(Color color)
        {
            Color = color;
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return product.Color.Equals(Color);
        }
    }
}