using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Adapter.Example1
{
    /// <summary>
    /// The Adaptee contains some useful behavior, but its interface is
    /// incompatible with the existing client code. 
    /// The Adaptee needs some adaptation before the client code can use it.
    /// </summary>
    public class Adaptee
    {
        public string GetSpecificRequest()
        {
            return "Specific request.";
        }
    }
}
