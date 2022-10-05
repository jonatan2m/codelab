using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Memento.Example1
{
    public interface IDbCommand
    {
        void Execute(string text);
        void Undo();
    }
}
