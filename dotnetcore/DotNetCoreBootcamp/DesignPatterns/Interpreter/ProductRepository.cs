using System.Collections.Generic;

namespace DesignPatterns.Interpreter
{
    public class ProductRepository
    {
        readonly List<Product> _products = new List<Product>();

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public IReadOnlyCollection<Product> Products()
        {
            return _products.AsReadOnly();
        }
    }
}