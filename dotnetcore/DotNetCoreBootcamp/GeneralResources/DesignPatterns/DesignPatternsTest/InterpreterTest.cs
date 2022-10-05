using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using DesignPatterns.Interpreter;
using DesignPatterns.Interpreter.RomanNumbers;
using Xunit;

namespace DesignPatternsTest
{
    public class InterpreterTest
    {
        private ProductFinder finder;

        Product fireTruck =
            new Product("f1234", "Fire Truck", Color.Red, 8.95f, ProductSize.MEDIUM);
        Product barbieClassic =
            new Product("b7654", "Barbie Classic", Color.Yellow, 15.95f, ProductSize.SMALL);
        Product frisbee =
            new Product("f4321", "Frisbee", Color.Pink, 9.99f, ProductSize.LARGE);
        Product baseball =
            new Product("b2343", "Baseball", Color.White, 8.95f, ProductSize.NOT_APPLICABLE);
        Product toyConvertible =
            new Product("p1112", "Toy Porsche Convertible", Color.Red, 230.00f, ProductSize.NOT_APPLICABLE);

        public InterpreterTest()
        {
            finder = new ProductFinder(CreateProductRepository());
        }

        private ProductRepository CreateProductRepository()
        {
            var productRepository = new ProductRepository();
            productRepository.Add(fireTruck);
            productRepository.Add(barbieClassic);
            productRepository.Add(frisbee);
            productRepository.Add(baseball);
            productRepository.Add(toyConvertible);
            return productRepository;
        }

        [Fact]
        public void Interpreter_Product_FindByColor()
        {
            List<Product> products = finder.ByColor(Color.Red);

            Assert.Equal(2, products.Count);
            Assert.Contains(products, x => x.Name.Contains("Fire"));
        }

        [Fact]
        public void Interpreter_Product_FindByColorAndBelowPrice()
        {
            List<Product> products = finder.ByColorAndBelowPrice(Color.Red, 9);

            Assert.Equal(1, products.Count);
            Assert.Contains(products, x => x.Name.Contains("Fire"));
        }

        [Fact]
        public void Interpreter_Roman_Number()
        {
            var roman = "CC";

            var number = DesignPatterns.Interpreter.RomanNumbers.Context.Run(roman);

            Assert.Equal(200, number);
        }
    }
}
