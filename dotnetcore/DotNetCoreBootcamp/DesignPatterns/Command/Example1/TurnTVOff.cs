using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// COMMAND
    /// </summary>
    public class TurnTVOff : ICommand
    {
        IElectronicDevice device;

        public TurnTVOff(IElectronicDevice newDevice)
        {
            device = newDevice;
        }

        public void Execute()
        {
            device.Off();
        }

        public void Undo()
        {
            device.On();
        }
    }
}
