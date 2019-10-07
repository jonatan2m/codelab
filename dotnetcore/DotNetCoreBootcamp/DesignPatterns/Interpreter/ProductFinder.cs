using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DesignPatterns.Interpreter
{
    public class ProductFinder
    {
        private readonly ProductRepository _repository;

        public ProductFinder(ProductRepository repository)
        {
            _repository = repository;
        }

        public List<Product> ByColor(Color color)
        {
            ColorSpec spec = new ColorSpec(color);

            return _repository.Products()
                .Where(spec.IsSatisfiedBy)
                .ToList();
        }

        public List<Product> ByColorAndBelowPrice(Color color, int price)
        {
            ColorSpec colorSpec = new ColorSpec(color);
            BelowPriceSpec belowPriceSpec = new BelowPriceSpec(price);
            AndSpec andSpec = new AndSpec(colorSpec, belowPriceSpec);

            return _repository.Products()
                .Where(andSpec.IsSatisfiedBy)
                .ToList();
        }
    }
}
