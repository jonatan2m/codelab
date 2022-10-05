using System;
namespace DesignPatterns.Command.Example1
{
    public interface IElectronicDevice
    {
        void On();
        void Off();
        void VolumeUp();
        void VolumenDown();
    }
}
