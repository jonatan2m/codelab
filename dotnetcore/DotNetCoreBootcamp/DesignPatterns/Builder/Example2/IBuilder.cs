using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Builder.Example2
{
    /// <summary>
    /// The Builder interface specifies methods for creating the different parts
    /// of the Product objects.
    /// </summary>
    public interface IBuilder
    {
        void BuildPartA();

        void BuildPartB();

        void BuildPartC();
    }
}
