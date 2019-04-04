using System;

namespace DesignPatterns.Builder.Example2
{
    /// <summary>
    /// The Concrete Builder classes follow the Builder interface and provide
    /// specific implementations of the building steps. Your program may have
    /// several variations of Builders, implemented differently.
    /// </summary>
    public class ConcreteBuilder : IBuilder
    {
        private Product _product;

        public ConcreteBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _product = new Product();
        }

        public void BuildPartA()
        {
            this._product.Add("PartA1");
        }

        public void BuildPartB()
        {
            this._product.Add("PartB1");
        }

        public void BuildPartC()
        {
            this._product.Add("PartC1");
        }

        /// <summary>
        /// Concrete Builders are supposed to provide their own methods for
        /// retrieving results. That's because various types of builders may
        /// create entirely different products that don't follow the same
        /// interface. Therefore, such methods cannot be declared in the base
        /// Builder interface (at least in a statically typed programming
        /// language).
        ///
        /// Usually, after returning the end result to the client, a builder
        /// instance is expected to be ready to start producing another product.
        /// That's why it's a usual practice to call the reset method at the end
        /// of the `GetProduct` method body. However, this behavior is not
        /// mandatory, and you can make your builders wait for an explicit reset
        /// call from the client code before disposing of the previous result.
        /// </summary>
        /// <returns></returns>
        public Product GetProduct()
        {
            Product result = this._product;

            this.Reset();

            return result;
        }
    }
}
