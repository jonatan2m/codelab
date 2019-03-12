using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example2
{
    public interface IAppCommand
    {
        void Execute(string text);
        void Undo();
    }
}
