using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// Each command you want to issue will implement
    /// the Command interface
    /// </summary>
    public interface ICommand
    {
        void Execute();
        
        // You may want to offer the option to undo a command
        void Undo();
    }
}
