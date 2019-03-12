using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// COMMAND
    /// </summary>
    public class TurnTVUp : ICommand
    {
        IElectronicDevice device;

        public TurnTVUp(IElectronicDevice newDevice)
        {
            device = newDevice;
        }

        public void Execute()
        {
            device.VolumeUp();
        }

        public void Undo()
        {
            device.VolumenDown();
        }
    }
}
