using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// COMMAND
    /// </summary>
    public class TurnTVOn : ICommand
    {
        IElectronicDevice device;

        public TurnTVOn(IElectronicDevice newDevice)
        {
            device = newDevice;
        }

        public void Execute()
        {
            device.On();
        }

        public void Undo()
        {
            device.Off();
        }
    }
}
