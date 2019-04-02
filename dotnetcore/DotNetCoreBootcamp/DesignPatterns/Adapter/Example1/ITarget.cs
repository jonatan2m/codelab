using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Adapter.Example1
{
    // The Target defines the domain-specific interface used by the client code.
    public interface ITarget
    {
        string GetRequest();
    }
}
