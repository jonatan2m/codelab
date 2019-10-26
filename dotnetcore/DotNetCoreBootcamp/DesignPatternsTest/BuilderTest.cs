using DesignPatterns.Builder.Example2;
using DesignPatterns.Builder.GiftExample;
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
        public void Builder_GiftExample_Test1()
        {
            var builder = new GiftBuilder();
        }
    }    
}

