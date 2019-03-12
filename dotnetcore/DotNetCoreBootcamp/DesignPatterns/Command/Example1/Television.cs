using System;
namespace DesignPatterns.Command.Example1
{
    /// <summary>
    /// RECEIVER
    /// </summary>
    public class Television : IElectronicDevice
    {
        private int volume = 0;

        public void Off()
        {
            throw new NotImplementedException();
        }

        public void On()
        {
            throw new NotImplementedException();
        }

        public void VolumenDown()
        {
            throw new NotImplementedException();
        }

        public void VolumeUp()
        {
            throw new NotImplementedException();
        }
    }
}
