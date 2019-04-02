using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Decorator.Example1
{
    /// <summary>
    /// The base Component interface defines operations that can be altered by decorators.
    /// </summary>
    public abstract class Component
    {
        public abstract string Operation();
    }

}
