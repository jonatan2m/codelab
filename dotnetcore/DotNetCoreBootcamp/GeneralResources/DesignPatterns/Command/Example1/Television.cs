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
            Console.WriteLine("TV is off");
        }

        public void On()
        {
            Console.WriteLine("TV is on");            
        }

        public void VolumenDown()
        {
            volume--;
            Console.WriteLine($"TV volume is at: {volume}");            
        }

        public void VolumeUp()
        {
            volume++;
            Console.WriteLine($"TV volume is at: {volume}");                        
        }
    }
}
