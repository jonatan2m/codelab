using DesignPatterns.Builder.Example2;
using DesignPatterns.Builder.FacetedBuilder;
using DesignPatterns.Builder.FluentBuilder;
using DesignPatterns.Builder.ProductStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DesignPatternsTest
{
    public class BuilderTest
    {
        [Fact]
        public void Builder_Example1()
        {
            var result = DesignPatterns.Builder.Example1.UsingUriBuilder.DotNetExample("https", "localhost", 8080);

            Assert.Equal("https://localhost:8080/", result);
        }

        [Fact]
        public void Builder_Example2()
        {
            var builder = new ConcreteBuilder();
            var director = new Director();
            director.Builder = builder;

            director.buildMinimalViableProduct();
            var product = builder.GetProduct();
            var result = product.ListParts();
            Assert.Equal("PartA1", result);

            director.buildFullFeaturedProduct();
            product = builder.GetProduct();
            result = product.ListParts();

            Assert.Equal("PartA1, PartB1, PartC1", result);
        }

        [Fact]
        public void Builder_ProductStock_Test1()
        {
            var products = new List<DesignPatterns.Builder.ProductStock.Product>
            {
                new DesignPatterns.Builder.ProductStock.Product{Name = "Monitor", Price = 200.50},
                new DesignPatterns.Builder.ProductStock.Product{Name = "Mouse", Price = 20.41},
                new DesignPatterns.Builder.ProductStock.Product{Name = "Keyboard", Price = 30.15},
            };

            var builder = new ProductStockReportBuilder(products);
            var director = new ProductStockReportDirector(builder);
            director.BuildStockReport();

            var report = builder.GetReport();
        }

        [Fact]
        public void FluentBuilder_EmployeeBuilder_Test1()
        {
            var emp = EmployeeBuilderDirector
                .NewEmployee
                .AtPosition("Software Developer")
                .WithSalary(3500)
                .SetName("Maks")
                .Build();

            Assert.Equal("Software Developer", emp.Position);
        }

        [Fact]
        public void FacetedBuilder_CarBuilder_Test1()
        {
            var builder = new CarBuilderFacade();
            var car = builder
                .Info
                    .WithColor("Black")
                    .WithType("Sedan")
                    .WithNumberOfDoors(4)
                .Built
                    .InCity("Rio de Janeiro")
                    .AtAddress("Rua camerino, 127")
                .Build();

            Assert.Equal("Rio de Janeiro", car.City);
        }
    }
}

