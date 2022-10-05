using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Decorator.Example1
{
    /// <summary>
    /// The base Decorator class follows the same interface as the other components.
    /// The primary purpose of this class is to define the wrapping interface for all concrete decorators.
    /// The default implementation of the wrapping code might
    /// include a field for storing a wrapped component and the means to initialize it.
    /// </summary>
    public abstract class Decorator : Component
    {
        protected Component _component;

        public Decorator(Component component)
        {
            this._component = component;
        }

        public void SetComponent(Component component)
        {
            this._component = component;
        }

        /// <summary>
        /// The Decorator delegates all work to the wrapped component.
        /// </summary>
        /// <returns></returns>
        public override string Operation()
        {
            if (this._component != null)
            {
                return this._component.Operation();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
