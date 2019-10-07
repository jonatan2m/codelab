using System.Drawing;

namespace DesignPatterns.Interpreter
{
    public class Product
    {
        public Color Color { get; set; }
        public float Price { get; set; }
        public ProductSize Size { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Product(string code, string name, Color color, float price, ProductSize size)
        {
            Code = code;
            Name = name;
            Color = color;
            Price = price;
            Size = size;
        }
    }
}