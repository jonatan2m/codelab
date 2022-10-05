using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// This is known as the invoker
    /// It has a method press() that when executed
    /// causes the execute method to be called
    /// 
    /// The execute method for the Command interface then calls
    /// the method assigned in the class that implements the Command interface
    /// </summary>
    public class DeviceButton
    {
        ICommand command;
        public DeviceButton(ICommand newCommand)
        {
            command = newCommand;
        }

        public void Press()
        {
            command.Execute();
        }

        public void PressUndo()
        {
            command.Undo();
        }
    }
}
