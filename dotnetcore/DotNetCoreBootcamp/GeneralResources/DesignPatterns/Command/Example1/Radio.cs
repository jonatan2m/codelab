using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    public class Radio : IElectronicDevice
    {
        private int volume = 0;

        public void Off()
        {
            Console.WriteLine("Radio is off");
        }

        public void On()
        {
            Console.WriteLine("Radio is on");
        }

        public void VolumenDown()
        {
            volume--;
            Console.WriteLine($"Radio volume is at: {volume}");
        }

        public void VolumeUp()
        {
            volume++;
            Console.WriteLine($"Radio volume is at: {volume}");
        }
    }
}
