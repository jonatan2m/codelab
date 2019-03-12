using System.Collections.Generic;

namespace DesignPatterns.Command.Example1
{
    public class TurnItAllOff : ICommand
    {
        IEnumerable<IElectronicDevice> devices;

        public TurnItAllOff(IEnumerable<IElectronicDevice> newDevices)
        {
            devices = newDevices;
        }

        public void Execute()
        {
            foreach (var device in devices)
            {
                device.Off();
            }
        }

        public void Undo()
        {
            foreach (var device in devices)
            {
                device.On();
            }            
        }
    }
}